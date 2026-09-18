using System;
using System.Collections.Generic;

namespace Veterinaria.Logica
{
    public class Cita
    {
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string NumeroCarnet { get; set; }

        // Esta bandera es la que usa Doctor.BuscarCitaDeHoy() para saber si
        // una cita ya fue resuelta o todavía sigue pendiente.
        public bool Atendida { get; set; }

        // Una cita puede tener VARIOS procedimientos (por eso es lista),
        // pero solo UNA receta (por eso es un solo objeto, que puede quedar
        // en null si el doctor todavía no la ha extendido).
        public List<Procedimiento> Procedimientos { get; set; }
        public Receta Receta { get; set; }

        public Cita(DateTime fecha, string hora, string numeroCarnet)
        {
            Fecha = fecha;
            Hora = hora;
            NumeroCarnet = numeroCarnet;
            Atendida = false;
            Procedimientos = new List<Procedimiento>();
            Receta = null;
        }
    }
}
