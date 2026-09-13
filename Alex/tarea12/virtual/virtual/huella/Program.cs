using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("\nMENÚ");
            Console.WriteLine("1. Generar Diagnóstico de Entrega");
            Console.WriteLine("2. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    huella();
                    break;
                case "2":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción no reconocida o no implementada en este ejemplo.");
                    break;
            }
        }
    }

    static void huella()
    {
        try
        {
            string nombreArchivo = "huelladigital.log";
            string rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);

            using (StreamWriter writer = new StreamWriter(rutaCompleta, false))
            {
                writer.WriteLine("DIAGNÓSTICO DE ENTREGA");
                writer.WriteLine($"Nombre de máquina (MachineName): {Environment.MachineName}");
                writer.WriteLine($"Versión de SO (OSVersion): {Environment.OSVersion}");
                writer.WriteLine($"Usuario (UserName): {Environment.UserName}");
                writer.WriteLine($"Fecha y hora de ejecución: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
               
            }

            Console.WriteLine($"\nDiagnóstico generado exitosamente en: {rutaCompleta}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error al generar el diagnóstico: {ex.Message}");
        }
    }
}
