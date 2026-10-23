using System;

namespace Veterinaria.Logica
{
    // Esta clase es pequeña a propósito. Su único trabajo es recordar
    // CUÁNDO empezó la sesión y, cuando se le pida cerrarla, calcular todo
    // lo demás apoyándose en lo que ya construimos en Usuario y
    // GestorArchivos. No repite ninguna lógica que ya exista en otro lado.
    public class RegistroSesion
    {
        // Guardamos una referencia al Usuario que inició sesión (puede ser
        // un Recepcionista, un Doctor o un Desarrollador, no importa cuál).
        private Usuario usuario;
        private DateTime horaInicio;

        public RegistroSesion(Usuario usuario)
        {
            this.usuario = usuario;
        }

        // Se llama justo en el instante en que el login fue exitoso, antes
        // de que la persona empiece a usar el menú de su modo.
        public void IniciarSesion()
        {
            horaInicio = DateTime.Now;
        }

        // Se llama cuando la persona cierra sesión (o cuando el programa
        // termina). Aquí es donde se junta todo lo que armamos antes:
        public void CerrarSesion()
        {
            DateTime horaCierre = DateTime.Now;

            // Este es el momento en el que el polimorfismo que dejamos
            // preparado en Usuario/Desarrollador realmente "paga": esta
            // clase NUNCA pregunta "¿eres Desarrollador? ¿eres Israel?".
            // Simplemente llama a ObtenerLineaLog(...), y cada objeto sabe
            // solo armar su propia línea correctamente (con o sin
            // contraseña) según su propio tipo.
            string linea = usuario.ObtenerLineaLog(horaInicio, horaCierre);
            string nombreArchivo = usuario.ObtenerNombreArchivoLog();

            GestorArchivos.GuardarLineaLog(nombreArchivo, linea);
        }
    }
}
