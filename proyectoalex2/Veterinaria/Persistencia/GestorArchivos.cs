using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Veterinaria.Logica
{
    // Misma clase y MISMOS metodos publicos que antes, pero ahora guardan
    // y leen de SQLite en vez de archivos .txt. Por eso Program.cs, Doctor,
    // Recepcionista, Desarrollador y RegistroSesion no cambian.
    public static class GestorArchivos
    {
        private const string FormatoFecha = "yyyy-MM-dd";

        // ---------------------------------------------------------------
        // CARNET
        // ---------------------------------------------------------------

        // Guarda el carnet completo con todas sus citas, recetas y
        // procedimientos. Funciona igual que antes (reescribir todo el
        // carnet), pero dentro de una transaccion: o se guarda TODO o no
        // se guarda NADA.
        public static void GuardarCarnet(Carnet carnet)
        {
            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteTransaction transaccion = conexion.BeginTransaction();

            try
            {
                // 1. Si el carnet existe lo actualiza, si no, lo crea.
                //    (No se usa INSERT OR REPLACE porque ese borraria el
                //    carnet y, por el CASCADE, tambien sus citas.)
                using (SqliteCommand cmd = conexion.CreateCommand())
                {
                    cmd.Transaction = transaccion;
                    cmd.CommandText = @"
INSERT INTO Carnets (NumeroCarnet, NombreAnimal, Especie, DpiDueno, Celular, Direccion)
VALUES ($numero, $animal, $especie, $dpi, $celular, $direccion)
ON CONFLICT(NumeroCarnet) DO UPDATE SET
    NombreAnimal = excluded.NombreAnimal,
    Especie      = excluded.Especie,
    DpiDueno     = excluded.DpiDueno,
    Celular      = excluded.Celular,
    Direccion    = excluded.Direccion;";
                    cmd.Parameters.AddWithValue("$numero", carnet.NumeroCarnet);
                    cmd.Parameters.AddWithValue("$animal", carnet.NombreAnimal);
                    cmd.Parameters.AddWithValue("$especie", carnet.Especie);
                    cmd.Parameters.AddWithValue("$dpi", carnet.DpiDueno);
                    cmd.Parameters.AddWithValue("$celular", carnet.Celular);
                    cmd.Parameters.AddWithValue("$direccion", carnet.Direccion);
                    cmd.ExecuteNonQuery();
                }

                // 2. Borra las citas viejas de este carnet. Las recetas y
                //    procedimientos se borran solos (ON DELETE CASCADE).
                using (SqliteCommand cmd = conexion.CreateCommand())
                {
                    cmd.Transaction = transaccion;
                    cmd.CommandText = "DELETE FROM Citas WHERE NumeroCarnet = $numero;";
                    cmd.Parameters.AddWithValue("$numero", carnet.NumeroCarnet);
                    cmd.ExecuteNonQuery();
                }

                // 3. Vuelve a insertar todas las citas que hay en memoria.
                foreach (Cita cita in carnet.Citas)
                {
                    long idCita;

                    using (SqliteCommand cmd = conexion.CreateCommand())
                    {
                        cmd.Transaction = transaccion;
                        cmd.CommandText = @"
INSERT INTO Citas (NumeroCarnet, Fecha, Hora, Atendida)
VALUES ($numero, $fecha, $hora, $atendida);
SELECT last_insert_rowid();";
                        cmd.Parameters.AddWithValue("$numero", carnet.NumeroCarnet);
                        cmd.Parameters.AddWithValue("$fecha", cita.Fecha.ToString(FormatoFecha, CultureInfo.InvariantCulture));
                        cmd.Parameters.AddWithValue("$hora", cita.Hora);
                        cmd.Parameters.AddWithValue("$atendida", cita.Atendida ? 1 : 0);
                        idCita = Convert.ToInt64(cmd.ExecuteScalar());
                    }

                    if (cita.Receta != null)
                    {
                        using (SqliteCommand cmd = conexion.CreateCommand())
                        {
                            cmd.Transaction = transaccion;
                            cmd.CommandText = @"
INSERT INTO Recetas (IdCita, Medicamento, Dosis, Indicaciones)
VALUES ($idCita, $medicamento, $dosis, $indicaciones);";
                            cmd.Parameters.AddWithValue("$idCita", idCita);
                            cmd.Parameters.AddWithValue("$medicamento", cita.Receta.Medicamento);
                            cmd.Parameters.AddWithValue("$dosis", cita.Receta.Dosis);
                            cmd.Parameters.AddWithValue("$indicaciones", cita.Receta.Indicaciones);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    foreach (Procedimiento procedimiento in cita.Procedimientos)
                    {
                        using (SqliteCommand cmd = conexion.CreateCommand())
                        {
                            cmd.Transaction = transaccion;
                            cmd.CommandText = @"
INSERT INTO Procedimientos (IdCita, Descripcion, Fecha)
VALUES ($idCita, $descripcion, $fecha);";
                            cmd.Parameters.AddWithValue("$idCita", idCita);
                            cmd.Parameters.AddWithValue("$descripcion", procedimiento.Descripcion);
                            cmd.Parameters.AddWithValue("$fecha", procedimiento.Fecha.ToString(FormatoFecha, CultureInfo.InvariantCulture));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        // Lee el carnet y reconstruye el objeto completo (Citas, Recetas y
        // Procedimientos). Devuelve null si el carnet no existe.
        public static Carnet LeerCarnet(string numeroCarnet)
        {
            using SqliteConnection conexion = BaseDatos.ObtenerConexion();

            Carnet carnet = null;

            // 1. Datos del carnet
            using (SqliteCommand cmd = conexion.CreateCommand())
            {
                cmd.CommandText = @"SELECT NumeroCarnet, NombreAnimal, Especie, DpiDueno, Celular, Direccion
                                    FROM Carnets WHERE NumeroCarnet = $numero;";
                cmd.Parameters.AddWithValue("$numero", numeroCarnet);

                using (SqliteDataReader lector = cmd.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        carnet = new Carnet(
                            lector.GetString(0), lector.GetString(1), lector.GetString(2),
                            lector.GetString(3), lector.GetString(4), lector.GetString(5));
                    }
                }
            }

            if (carnet == null)
            {
                return null;
            }

            // 2. Citas. Guardamos cada una en un diccionario por su IdCita
            //    para poder colgarle despues su receta y sus procedimientos.
            Dictionary<long, Cita> citasPorId = new Dictionary<long, Cita>();

            using (SqliteCommand cmd = conexion.CreateCommand())
            {
                cmd.CommandText = @"SELECT IdCita, Fecha, Hora, Atendida
                                    FROM Citas WHERE NumeroCarnet = $numero ORDER BY IdCita;";
                cmd.Parameters.AddWithValue("$numero", numeroCarnet);

                using (SqliteDataReader lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Cita cita = new Cita(
                            DateTime.Parse(lector.GetString(1), CultureInfo.InvariantCulture),
                            lector.GetString(2),
                            numeroCarnet);
                        cita.Atendida = lector.GetInt32(3) == 1;
                        carnet.Citas.Add(cita);
                        citasPorId[lector.GetInt64(0)] = cita;
                    }
                }
            }

            // 3. Recetas de todas las citas de este carnet
            using (SqliteCommand cmd = conexion.CreateCommand())
            {
                cmd.CommandText = @"SELECT r.IdCita, r.Medicamento, r.Dosis, r.Indicaciones
                                    FROM Recetas r
                                    INNER JOIN Citas c ON c.IdCita = r.IdCita
                                    WHERE c.NumeroCarnet = $numero;";
                cmd.Parameters.AddWithValue("$numero", numeroCarnet);

                using (SqliteDataReader lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        citasPorId[lector.GetInt64(0)].Receta = new Receta(
                            lector.GetString(1), lector.GetString(2), lector.GetString(3));
                    }
                }
            }

            // 4. Procedimientos de todas las citas de este carnet
            using (SqliteCommand cmd = conexion.CreateCommand())
            {
                cmd.CommandText = @"SELECT p.IdCita, p.Descripcion, p.Fecha
                                    FROM Procedimientos p
                                    INNER JOIN Citas c ON c.IdCita = p.IdCita
                                    WHERE c.NumeroCarnet = $numero
                                    ORDER BY p.IdProcedimiento;";
                cmd.Parameters.AddWithValue("$numero", numeroCarnet);

                using (SqliteDataReader lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        citasPorId[lector.GetInt64(0)].Procedimientos.Add(
                            new Procedimiento(
                                lector.GetString(1),
                                DateTime.Parse(lector.GetString(2), CultureInfo.InvariantCulture)));
                    }
                }
            }

            return carnet;
        }

        // Antes dejaba el .txt en blanco (y por eso LeerCarnet devolvia null).
        // Ahora borra el carnet y, por el CASCADE, todas sus citas, recetas
        // y procedimientos. El resultado para el programa es el mismo.
        public static void BorrarContenidoCarnet(string numeroCarnet)
        {
            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "DELETE FROM Carnets WHERE NumeroCarnet = $numero;";
            cmd.Parameters.AddWithValue("$numero", numeroCarnet);
            cmd.ExecuteNonQuery();
        }

        // ---------------------------------------------------------------
        // USUARIOS (recepcionistas y doctores)
        // ---------------------------------------------------------------

        // Si ya existe un usuario con ese nombre y tipo, no hace nada.
        public static void GuardarUsuario(string nombre, string contrasena, string tipo)
        {
            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand cmd = conexion.CreateCommand();
            cmd.CommandText = @"INSERT OR IGNORE INTO Usuarios (Nombre, Contrasena, Tipo)
                                VALUES ($nombre, $contrasena, $tipo);";
            cmd.Parameters.AddWithValue("$nombre", nombre);
            cmd.Parameters.AddWithValue("$contrasena", contrasena);
            cmd.Parameters.AddWithValue("$tipo", tipo);
            cmd.ExecuteNonQuery();
        }

        // Mismo formato de antes: [0]=nombre, [1]=contrasena, [2]=tipo.
        public static List<string[]> LeerUsuarios()
        {
            List<string[]> usuarios = new List<string[]>();

            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT Nombre, Contrasena, Tipo FROM Usuarios;";

            using SqliteDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                usuarios.Add(new string[]
                {
                    lector.GetString(0), lector.GetString(1), lector.GetString(2)
                });
            }

            return usuarios;
        }

        public static void ActualizarContrasena(string nombre, string tipo, string nuevaContrasena)
        {
            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "UPDATE Usuarios SET Contrasena = $contrasena WHERE Nombre = $nombre AND Tipo = $tipo;";
            cmd.Parameters.AddWithValue("$contrasena", nuevaContrasena);
            cmd.Parameters.AddWithValue("$nombre", nombre);
            cmd.Parameters.AddWithValue("$tipo", tipo);
            cmd.ExecuteNonQuery();
        }

        // ---------------------------------------------------------------
        // DESARROLLADORES (modo 3)
        // ---------------------------------------------------------------

        public static void GuardarDesarrollador(string nombre, string contrasena)
        {
            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "INSERT OR IGNORE INTO Desarrolladores (Nombre, Contrasena) VALUES ($nombre, $contrasena);";
            cmd.Parameters.AddWithValue("$nombre", nombre);
            cmd.Parameters.AddWithValue("$contrasena", contrasena);
            cmd.ExecuteNonQuery();
        }

        // Mismo formato de antes: [0]=nombre, [1]=contrasena.
        public static List<string[]> LeerDesarrolladores()
        {
            List<string[]> desarrolladores = new List<string[]>();

            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT Nombre, Contrasena FROM Desarrolladores;";

            using SqliteDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                desarrolladores.Add(new string[] { lector.GetString(0), lector.GetString(1) });
            }

            return desarrolladores;
        }

        // ---------------------------------------------------------------
        // LOGS (registro de sesion)
        // ---------------------------------------------------------------

        // RegistroSesion todavia manda un nombre tipo "israel.txt". Le
        // quitamos la extension y guardamos solo "israel" en la columna
        // Usuario; cada llamada agrega una fila nueva (historial completo).
        public static void GuardarLineaLog(string nombreArchivoLog, string linea)
        {
            string usuario = Path.GetFileNameWithoutExtension(nombreArchivoLog);

            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand cmd = conexion.CreateCommand();
            cmd.CommandText = "INSERT INTO Logs (Usuario, Linea) VALUES ($usuario, $linea);";
            cmd.Parameters.AddWithValue("$usuario", usuario);
            cmd.Parameters.AddWithValue("$linea", linea);
            cmd.ExecuteNonQuery();
        }
    }
}
