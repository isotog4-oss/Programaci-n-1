namespace Veterinaria.Logica
{
    public class Receta
    {
        public string Medicamento { get; set; }
        public string Dosis { get; set; }
        public string Indicaciones { get; set; }

        public Receta(string medicamento, string dosis, string indicaciones)
        {
            Medicamento = medicamento;
            Dosis = dosis;
            Indicaciones = indicaciones;
        }

        // "Descripcion" no es un dato que se guarde por separado: se arma
        // solo, cada vez que alguien la pide, juntando los tres campos de
        // arriba en un solo texto legible. Esto es lo que usan
        // Recepcionista.VerReceta() y Doctor.VerReporte() para mostrar la
        // receta en una sola línea sin tener que repetir esa misma lógica
        // de armado de texto en dos lugares distintos.
        public string Descripcion
        {
            get
            {
                return "Medicamento: " + Medicamento +
                       " | Dosis: " + Dosis +
                       " | Indicaciones: " + Indicaciones;
            }
        }
    }
}
