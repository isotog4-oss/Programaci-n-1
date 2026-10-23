using System;
using System.Collections.Generic;
using System.IO;

namespace Veterinaria.Logica
{
    // Es "static" porque no necesitamos crear un objeto GestorArchivos para
    // usarla (no tiene sentido tener "varios" gestores de archivos, solo
    // hay UNA forma de guardar y leer). Se usa siempre como
    // GestorArchivos.MetodoQueSea(...), sin "new".
    public static class GestorArchivos
    {
        // Rutas centralizadas aquí arriba: si algún día cambias dónde vive
        // la carpeta Datos, solo tocas estas líneas.
        private static readonly string RutaCarnet = "Datos/Carnet/";
        private static readonly string RutaUsuarios = "Datos/Usuarios/usuarios.txt";
        private static readonly string RutaDesarrolladores = "Datos/Usuarios/desarrolladores.txt";
        private static readonly string RutaLogs = "Datos/Logs/";

        // Este constructor "static" se ejecuta UNA sola vez, la primera vez
        // que algo toca la clase GestorArchivos. Lo usamos para asegurarnos
        // de que las carpetas existan antes de intentar escribir cualquier
        // archivo. Directory.CreateDirectory no da error si ya existe.
        static GestorArchivos()
        {
            Directory.CreateDirectory(RutaCarnet);
            Directory.CreateDirectory("Datos/Usuarios/");
            Directory.CreateDirectory(RutaLogs);
        }

        // Ayudante interno: de una línea "Clave=Valor" devuelve solo lo que
        // hay después del "=". Lo usa toda la lectura de Carnet, así que lo
        // separamos en vez de repetir el mismo Split/Substring muchas veces.
        private static string ObtenerValor(string linea)
        {
            int posicionIgual = linea.IndexOf('=');
            return linea.Substring(posicionIgual + 1);
        }

        // ---------------------------------------------------------------
        // CARNET
        // ---------------------------------------------------------------

        // Convierte un Carnet completo (con todas sus citas, procedimientos
        // y recetas) en líneas de texto tipo "Clave=Valor", con marcadores
        // como "CITA" / "FIN_CITA" para saber dónde empieza y termina cada
        // bloque anidado.
        public static void GuardarCarnet(Carnet carnet)
        {
            string ruta = RutaCarnet + "carnet_" + carnet.NumeroCarnet + ".txt";
            List<string> lineas = new List<string>();

            lineas.Add("CARNET");
            lineas.Add("Numero=" + carnet.NumeroCarnet);
            lineas.Add("Animal=" + carnet.NombreAnimal);
            lineas.Add("Especie=" + carnet.Especie);
            lineas.Add("DPI=" + carnet.DpiDueno);
            lineas.Add("Celular=" + carnet.Celular);
            lineas.Add("Direccion=" + carnet.Direccion);

            foreach (Cita cita in carnet.Citas)
            {
                lineas.Add("CITA");
                lineas.Add("FechaCita=" + cita.Fecha.ToString("yyyy-MM-dd"));
                lineas.Add("HoraCita=" + cita.Hora);
                lineas.Add("AtendidaCita=" + cita.Atendida);

                if (cita.Receta != null)
                {
                    lineas.Add("RECETA");
                    lineas.Add("Medicamento=" + cita.Receta.Medicamento);
                    lineas.Add("Dosis=" + cita.Receta.Dosis);
                    lineas.Add("Indicaciones=" + cita.Receta.Indicaciones);
                    lineas.Add("FIN_RECETA");
                }

                foreach (Procedimiento procedimiento in cita.Procedimientos)
                {
                    lineas.Add("PROCEDIMIENTO");
                    lineas.Add("DescripcionProcedimiento=" + procedimiento.Descripcion);
                    lineas.Add("FechaProcedimiento=" + procedimiento.Fecha.ToString("yyyy-MM-dd"));
                    lineas.Add("FIN_PROCEDIMIENTO");
                }

                lineas.Add("FIN_CITA");
            }

            lineas.Add("FIN_CARNET");

            File.WriteAllLines(ruta, lineas);
        }

        // Hace el camino inverso: lee las líneas del .txt y reconstruye el
        // objeto Carnet (con sus Citas, Procedimientos y Recetas) en
        // memoria. Usamos una variable "modo" para saber en qué bloque
        // estamos leyendo en cada momento (es lo que se llama una "máquina
        // de estados" simple): así, una línea "FechaCita=" y una línea
        // "FechaProcedimiento=" nunca se pueden confundir, porque tienen
        // nombres de clave distintos Y además revisamos en qué "modo"
        // estamos antes de asignarlas.
        public static Carnet LeerCarnet(string numeroCarnet)
        {
            string ruta = RutaCarnet + "carnet_" + numeroCarnet + ".txt";

            if (!File.Exists(ruta))
            {
                return null;
            }

            string[] lineas = File.ReadAllLines(ruta);

            Carnet carnet = null;
            Cita citaActual = null;
            string modo = "";

            // Variables "temporales" para ir armando una Receta o un
            // Procedimiento mientras seguimos leyendo línea por línea,
            // hasta que llegue su "FIN_..." y ahí sí las convertimos en
            // el objeto final.
            string medicamento = "", dosis = "", indicaciones = "";
            string descripcionProcedimiento = "";
            DateTime fechaProcedimiento = DateTime.MinValue;

            foreach (string linea in lineas)
            {
                if (linea == "CARNET")
                {
                    modo = "CARNET";
                }
                else if (linea == "CITA")
                {
                    citaActual = new Cita(DateTime.MinValue, "", numeroCarnet);
                    modo = "CITA";
                }
                else if (linea == "RECETA")
                {
                    modo = "RECETA";
                }
                else if (linea == "FIN_RECETA")
                {
                    citaActual.Receta = new Receta(medicamento, dosis, indicaciones);
                    modo = "CITA";
                }
                else if (linea == "PROCEDIMIENTO")
                {
                    modo = "PROCEDIMIENTO";
                }
                else if (linea == "FIN_PROCEDIMIENTO")
                {
                    citaActual.Procedimientos.Add(
                        new Procedimiento(descripcionProcedimiento, fechaProcedimiento));
                    modo = "CITA";
                }
                else if (linea == "FIN_CITA")
                {
                    carnet.Citas.Add(citaActual);
                    citaActual = null;
                    modo = "CARNET";
                }
                else if (linea == "FIN_CARNET")
                {
                    modo = "";
                }
                else if (modo == "CARNET" && linea.StartsWith("Numero="))
                {
                    carnet = new Carnet(ObtenerValor(linea), "", "", "", "", "");
                }
                else if (modo == "CARNET" && linea.StartsWith("Animal="))
                {
                    carnet.NombreAnimal = ObtenerValor(linea);
                }
                else if (modo == "CARNET" && linea.StartsWith("Especie="))
                {
                    carnet.Especie = ObtenerValor(linea);
                }
                else if (modo == "CARNET" && linea.StartsWith("DPI="))
                {
                    carnet.DpiDueno = ObtenerValor(linea);
                }
                else if (modo == "CARNET" && linea.StartsWith("Celular="))
                {
                    carnet.Celular = ObtenerValor(linea);
                }
                else if (modo == "CARNET" && linea.StartsWith("Direccion="))
                {
                    carnet.Direccion = ObtenerValor(linea);
                }
                else if (modo == "CITA" && linea.StartsWith("FechaCita="))
                {
                    citaActual.Fecha = DateTime.Parse(ObtenerValor(linea));
                }
                else if (modo == "CITA" && linea.StartsWith("HoraCita="))
                {
                    citaActual.Hora = ObtenerValor(linea);
                }
                else if (modo == "CITA" && linea.StartsWith("AtendidaCita="))
                {
                    citaActual.Atendida = bool.Parse(ObtenerValor(linea));
                }
                else if (modo == "RECETA" && linea.StartsWith("Medicamento="))
                {
                    medicamento = ObtenerValor(linea);
                }
                else if (modo == "RECETA" && linea.StartsWith("Dosis="))
                {
                    dosis = ObtenerValor(linea);
                }
                else if (modo == "RECETA" && linea.StartsWith("Indicaciones="))
                {
                    indicaciones = ObtenerValor(linea);
                }
                else if (modo == "PROCEDIMIENTO" && linea.StartsWith("DescripcionProcedimiento="))
                {
                    descripcionProcedimiento = ObtenerValor(linea);
                }
                else if (modo == "PROCEDIMIENTO" && linea.StartsWith("FechaProcedimiento="))
                {
                    fechaProcedimiento = DateTime.Parse(ObtenerValor(linea));
                }
            }

            return carnet;
        }

        // Borra TODO el contenido del archivo de un carnet (lo deja en
        // blanco), tal como pediste para "restablecer usuario": es
        // deliberadamente más agresivo que ActualizarContrasena.
        public static void BorrarContenidoCarnet(string numeroCarnet)
        {
            string ruta = RutaCarnet + "carnet_" + numeroCarnet + ".txt";
            if (File.Exists(ruta))
            {
                File.WriteAllText(ruta, "");
            }
        }

        // ---------------------------------------------------------------
        // USUARIOS (recepcionistas y doctores)
        // ---------------------------------------------------------------

        // Estos son registros "planos" (una línea = un usuario), así que no
        // necesitan un formato de bloques como Carnet. Usamos "|" como
        // separador entre campos.
        public static void GuardarUsuario(string nombre, string contrasena, string tipo)
        {
            string linea = nombre + "|" + contrasena + "|" + tipo;
            File.AppendAllText(RutaUsuarios, linea + Environment.NewLine);
        }

        // Devuelve todos los usuarios como un arreglo de 3 posiciones por
        // línea: [0]=nombre, [1]=contrasena, [2]=tipo. Esto lo va a usar
        // Program.cs para validar el login sin tener que crear una clase
        // extra solo para "leer una línea".
        public static List<string[]> LeerUsuarios()
        {
            List<string[]> usuarios = new List<string[]>();

            if (!File.Exists(RutaUsuarios))
            {
                return usuarios;
            }

            foreach (string linea in File.ReadAllLines(RutaUsuarios))
            {
                if (linea.Trim() != "")
                {
                    usuarios.Add(linea.Split('|'));
                }
            }

            return usuarios;
        }

        // Cambia SOLO la contraseña de un usuario que ya existe, sin tocar
        // nada más del archivo.
        public static void ActualizarContrasena(string nombre, string tipo, string nuevaContrasena)
        {
            if (!File.Exists(RutaUsuarios))
            {
                return;
            }

            string[] lineas = File.ReadAllLines(RutaUsuarios);

            for (int i = 0; i < lineas.Length; i++)
            {
                string[] campos = lineas[i].Split('|');
                if (campos.Length == 3 && campos[0] == nombre && campos[2] == tipo)
                {
                    lineas[i] = nombre + "|" + nuevaContrasena + "|" + tipo;
                }
            }

            File.WriteAllLines(RutaUsuarios, lineas);
        }

        // ---------------------------------------------------------------
        // DESARROLLADORES (modo 3)
        // ---------------------------------------------------------------

        // Recibimos nombre y contraseña ya "sueltos" (no el objeto
        // Desarrollador completo) porque Contrasena es protected y esta
        // clase no forma parte de la jerarquía de Usuario, así que no
        // podría leerla directo de un objeto Desarrollador de todas formas.
        public static void GuardarDesarrollador(string nombre, string contrasena)
        {
            string linea = nombre + "|" + contrasena;
            File.AppendAllText(RutaDesarrolladores, linea + Environment.NewLine);
        }

        public static List<string[]> LeerDesarrolladores()
        {
            List<string[]> desarrolladores = new List<string[]>();

            if (!File.Exists(RutaDesarrolladores))
            {
                return desarrolladores;
            }

            foreach (string linea in File.ReadAllLines(RutaDesarrolladores))
            {
                if (linea.Trim() != "")
                {
                    desarrolladores.Add(linea.Split('|'));
                }
            }

            return desarrolladores;
        }

        // ---------------------------------------------------------------
        // LOGS (registro de sesión)
        // ---------------------------------------------------------------

        // Simplemente agrega una línea más al final del archivo de log de
        // ese usuario. Usamos AppendAllText (no WriteAllText) porque
        // queremos conservar todo el historial de sesiones anteriores, no
        // reemplazarlo cada vez.
        public static void GuardarLineaLog(string nombreArchivoLog, string linea)
        {
            string ruta = RutaLogs + nombreArchivoLog;
            File.AppendAllText(ruta, linea + Environment.NewLine);
        }
    }
}
