using Microsoft.Data.Sqlite;

namespace Veterinaria.Logica
{
    // Entrega conexiones a la base de datos SQLite. La base es un solo
    // archivo (veterinaria.db) que se crea solo la primera vez.
    public static class BaseDatos
    {
        // Por defecto el archivo queda en:
        //   C:\Users\<tu usuario>\AppData\Local\UMG\Veterinaria\veterinaria.db
        // Si quieres otra ubicacion, define la variable de entorno
        // VETERINARIA_DB con la ruta completa del archivo.
        public static string RutaBaseDatos { get; } =
            Environment.GetEnvironmentVariable("VETERINARIA_DB")
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "UMG",
                "Veterinaria",
                "veterinaria.db");

        // ForeignKeys = true es importante: sin esto SQLite ignora las
        // llaves foraneas y el borrado en cascada (carnet -> citas ->
        // recetas y procedimientos) no funcionaria.
        public static string CadenaConexion => new SqliteConnectionStringBuilder
        {
            DataSource = RutaBaseDatos,
            ForeignKeys = true
        }.ToString();

        // Devuelve la conexion YA abierta. Quien la pida debe usarla dentro
        // de un "using" para que se cierre sola al terminar.
        public static SqliteConnection ObtenerConexion()
        {
            SqliteConnection conexion = new SqliteConnection(CadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}
