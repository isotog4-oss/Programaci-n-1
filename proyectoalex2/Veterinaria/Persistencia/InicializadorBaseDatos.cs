using System.Reflection;
using Microsoft.Data.Sqlite;

namespace Veterinaria.Logica
{
    // Se ejecuta UNA vez al arrancar el programa. Crea la carpeta, el
    // archivo de base de datos y las tablas si todavia no existen. Si ya
    // existen, no toca nada.
    public static class InicializadorBaseDatos
    {
        public static void Inicializar()
        {
            string? directorio = Path.GetDirectoryName(BaseDatos.RutaBaseDatos);
            if (!string.IsNullOrEmpty(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            using SqliteConnection conexion = BaseDatos.ObtenerConexion();
            using SqliteCommand comando = conexion.CreateCommand();
            comando.CommandText = LeerScriptTablas();
            comando.ExecuteNonQuery();
        }

        // El script crear_tablas.sql viaja dentro del propio programa
        // (recurso incrustado), asi no depende de ningun archivo suelto.
        private static string LeerScriptTablas()
        {
            Assembly ensamblado = Assembly.GetExecutingAssembly();

            string recurso = ensamblado.GetManifestResourceNames()
                .Single(nombre => nombre.EndsWith("crear_tablas.sql", StringComparison.OrdinalIgnoreCase));

            using Stream stream = ensamblado.GetManifestResourceStream(recurso)
                ?? throw new InvalidOperationException("No se encontro el script de tablas.");
            using StreamReader lector = new StreamReader(stream);
            return lector.ReadToEnd();
        }
    }
}
