using System.Collections.Generic;

namespace Veterinaria.Logica
{
    public class Carnet
    {
        public string NumeroCarnet { get; set; }
        public string NombreAnimal { get; set; }
        public string Especie { get; set; }
        public string DpiDueno { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }

        // Un carnet puede acumular varias citas a lo largo del tiempo.
        // Esta lista es, dentro de un mismo objeto en memoria, la misma
        // relación "uno a muchos" de la que hablamos antes (carnet = llave
        // primaria, cada cita "pertenece" a este carnet).
        public List<Cita> Citas { get; set; }

        public Carnet(string numeroCarnet, string nombreAnimal, string especie,
                      string dpiDueno, string celular, string direccion)
        {
            NumeroCarnet = numeroCarnet;
            NombreAnimal = nombreAnimal;
            Especie = especie;
            DpiDueno = dpiDueno;
            Celular = celular;
            Direccion = direccion;

            // Inicializamos la lista vacía aquí mismo, en el constructor,
            // para que nunca exista un Carnet con Citas = null. Así, en
            // cualquier otra clase, siempre puedes hacer un foreach sobre
            // carnet.Citas sin miedo a que truene por ser null.
            Citas = new List<Cita>();
        }
    }
}
