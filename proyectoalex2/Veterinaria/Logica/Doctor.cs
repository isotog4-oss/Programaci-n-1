using System;

namespace Veterinaria.Logica
{
    public class Doctor : Usuario
    {
        public Doctor(string nombre, string contrasena) : base(nombre, contrasena)
        {
        }

        // Igual que en Recepcionista: el doctor también necesita poder
        // buscar un carnet, por ejemplo cuando quiere revisar otro
        // distinto al que el sistema le propuso automáticamente.
        public Carnet BuscarCarnet(string numeroCarnet)
        {
            return GestorArchivos.LeerCarnet(numeroCarnet);
        }

        // Este método es el que resuelve justo lo que pediste: cuando el
        // doctor entra, si hay una cita programada para HOY dentro de ese
        // carnet y todavía no fue atendida, se la devuelve para que el
        // sistema le pregunte "¿es esta la cita?". Si no hay ninguna,
        // devuelve null y el programa sabe que debe ofrecer "buscar otra".
        public Cita BuscarCitaDeHoy(Carnet carnet)
        {
            foreach (Cita cita in carnet.Citas)
            {
                if (cita.Fecha.Date == DateTime.Now.Date && !cita.Atendida)
                {
                    return cita;
                }
            }
            return null;
        }

        // Registrar QUÉ se hizo en la cita. Se guarda como un Procedimiento
        // separado (no como un texto suelto dentro de Cita) porque en una
        // sola visita puede haber varios procedimientos, y así cada uno
        // queda con su propia fecha y descripción.
        public void RegistrarProcedimiento(Cita cita, string descripcion)
        {
            Procedimiento nuevoProcedimiento = new Procedimiento(descripcion, DateTime.Now);
            cita.Procedimientos.Add(nuevoProcedimiento);
        }

        // Extender (crear) la receta de esta cita. La separamos de
        // RegistrarProcedimiento a propósito: son dos conceptos distintos
        // en tu descripción original (procedimiento realizado vs receta),
        // y además la receta es lo único de esto que Recepción también
        // puede ver, así que tiene sentido que viva en su propia clase.
        public void ExtenderReceta(Cita cita, string medicamento, string dosis, string indicaciones)
        {
            cita.Receta = new Receta(medicamento, dosis, indicaciones);
        }

        // Cuando el doctor termina de atender, se marca la cita como
        // "Atendida" y se guarda el carnet completo (con procedimientos y
        // receta ya incluidos) de vuelta al archivo .txt.
        public void FinalizarCita(Carnet carnet, Cita cita)
        {
            cita.Atendida = true;
            GestorArchivos.GuardarCarnet(carnet);
        }

        // El doctor sí puede ver el reporte completo (a diferencia de
        // Recepcionista, que solo veía la receta). Por eso este método
        // vive únicamente aquí, en Doctor, y arma un texto con fecha,
        // receta y todos los procedimientos de la cita.
        public string VerReporte(Cita cita)
        {
            string reporte = "Fecha de la cita: " + cita.Fecha.ToShortDateString() + "\n";
            reporte += "Receta: " + (cita.Receta != null ? cita.Receta.Descripcion : "Ninguna") + "\n";
            reporte += "Procedimientos realizados:\n";

            if (cita.Procedimientos.Count == 0)
            {
                reporte += "- Todavía no se ha registrado ninguno.\n";
            }

            foreach (Procedimiento p in cita.Procedimientos)
            {
                reporte += "- " + p.Descripcion + " (" + p.Fecha.ToShortDateString() + ")\n";
            }

            return reporte;
        }
    }
}
