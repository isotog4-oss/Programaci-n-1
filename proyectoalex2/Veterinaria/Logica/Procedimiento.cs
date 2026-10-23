using System;

namespace Veterinaria.Logica
{
    // Clase deliberadamente pequeña y simple: solo guarda QUÉ se hizo y
    // CUÁNDO. No necesita más porque su único trabajo es ser un elemento
    // dentro de la lista Cita.Procedimientos.
    public class Procedimiento
    {
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public Procedimiento(string descripcion, DateTime fecha)
        {
            Descripcion = descripcion;
            Fecha = fecha;
        }
    }
}
