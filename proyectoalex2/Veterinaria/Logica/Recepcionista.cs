using System;

namespace Veterinaria.Logica
{
    // Recepcionista hereda de Usuario, así que automáticamente ya tiene
    // Nombre, Contrasena, ValidarContrasena() y ObtenerNombreArchivoLog()
    // sin tener que volver a escribirlos aquí.
    public class Recepcionista : Usuario
    {
        // "base(nombre, contrasena)" manda esos dos datos directo al
        // constructor de Usuario que ya escribimos. Recepcionista no
        // necesita guardar nada extra que Usuario no tenga, así que su
        // propio constructor queda vacío por dentro.
        public Recepcionista(string nombre, string contrasena) : base(nombre, contrasena)
        {
        }

        // Crea un carnet nuevo. Recibe los datos "sueltos" (strings, DateTime)
        // en vez de recibir ya un objeto Carnet armado, porque así se ve más
        // claro qué información exacta pide el modo recepción (justo lo que
        // tú mencionaste: nombre del animal, especie, DPI, celular, dirección).
        public Carnet CrearCarnet(string numeroCarnet, string nombreAnimal, string especie,
                                   string dpiDueno, string celular, string direccion)
        {
            Carnet nuevoCarnet = new Carnet(numeroCarnet, nombreAnimal, especie, dpiDueno, celular, direccion);

            // Recepcionista no sabe (ni le importa) CÓMO se escribe el archivo.
            // Solo le pide a GestorArchivos que lo guarde. Esto es la idea de
            // separar responsabilidades: Recepcionista sabe "qué" hacer,
            // GestorArchivos sabe "cómo" hacerlo.
            GestorArchivos.GuardarCarnet(nuevoCarnet);
            return nuevoCarnet;
        }

        // Busca un carnet ya existente por su número, para ver su cita o
        // modificarla.
        public Carnet BuscarCarnet(string numeroCarnet)
        {
            return GestorArchivos.LeerCarnet(numeroCarnet);
        }

        // Agenda una cita nueva dentro de un carnet ya existente.
        public void AgendarCita(Carnet carnet, DateTime fecha, string hora)
        {
            Cita nuevaCita = new Cita(fecha, hora, carnet.NumeroCarnet);
            carnet.Citas.Add(nuevaCita);
            GestorArchivos.GuardarCarnet(carnet);
        }

        // Cambia el día o el horario de una cita que ya existía.
        // No creamos una cita nueva, modificamos la misma para no perder
        // el resto de su información (por ejemplo si ya tuviera receta).
        public void CambiarCita(Carnet carnet, Cita cita, DateTime nuevaFecha, string nuevaHora)
        {
            cita.Fecha = nuevaFecha;
            cita.Hora = nuevaHora;
            GestorArchivos.GuardarCarnet(carnet);
        }

        // Ver la receta de una cita. OJO: devuelve un string, no el objeto
        // Receta completo. Esto refleja a propósito la regla que diste:
        // "en recepción solo puede VER la receta", no modificarla. Al
        // devolver solo texto, Recepcionista físicamente no tiene forma de
        // cambiar los datos de la receta, solo de leerlos.
        public string VerReceta(Cita cita)
        {
            if (cita.Receta == null)
            {
                return "No hay receta registrada para esta cita.";
            }
            return cita.Receta.Descripcion;
        }
    }
}
