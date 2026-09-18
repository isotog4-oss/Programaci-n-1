using System;

namespace Veterinaria.Logica
{
    public class Desarrollador : Usuario
    {
        // Esta bandera es la clave de toda la clase. Diferencia a Israel
        // (el desarrollador "raíz", el primero que existe) de cualquier
        // otro desarrollador que él mismo cree después. Es la única
        // diferencia real de comportamiento entre desarrolladores, así
        // que en vez de crear una clase aparte para Israel, basta con
        // esta bandera.
        public bool EsRaiz { get; set; }

        public Desarrollador(string nombre, string contrasena, bool esRaiz) : base(nombre, contrasena)
        {
            EsRaiz = esRaiz;
        }

        // Decide qué línea exacta se va a escribir en el archivo de Logs.
        // Aquí es donde se resuelve tu regla especial: Israel (EsRaiz)
        // nunca guarda su contraseña en el log, los demás desarrolladores
        // sí. Fíjate que la diferencia entre los dos "return" es
        // únicamente que uno incluye "Contrasena: ..." y el otro no.
        public string ObtenerLineaLog(DateTime inicio, DateTime cierre)
        {
            TimeSpan duracion = cierre - inicio;

            if (EsRaiz)
            {
                return "Nombre: " + Nombre +
                       " | Inicio: " + inicio +
                       " | Cierre: " + cierre +
                       " | Duracion: " + (int)duracion.TotalMinutes + " min";
            }

            return "Nombre: " + Nombre +
                   " | Contrasena: " + Contrasena +
                   " | Inicio: " + inicio +
                   " | Cierre: " + cierre +
                   " | Duracion: " + (int)duracion.TotalMinutes + " min";
        }

        // Solo Israel puede crear nuevos desarrolladores. En vez de confiar
        // en que Program.cs recuerde validar esto antes de llamar al
        // método, la regla se pone AQUÍ, adentro de la propia clase. Así,
        // sin importar desde dónde se llame este método en el futuro, la
        // regla de negocio no se puede saltar.
        public Desarrollador CrearDesarrollador(string nombre, string contrasena)
        {
            if (!EsRaiz)
            {
                throw new InvalidOperationException(
                    "Solo el desarrollador raiz (Israel) puede crear nuevos desarrolladores.");
            }

            Desarrollador nuevo = new Desarrollador(nombre, contrasena, false);
            GestorArchivos.GuardarDesarrollador(nuevo);
            return nuevo;
        }

        // Crear un usuario normal del sistema (recepcionista o doctor).
        // "tipo" es un texto simple ("Recepcionista" o "Doctor") que le
        // dice a GestorArchivos en qué lista guardarlo.
        public void CrearUsuario(string nombre, string contrasena, string tipo)
        {
            GestorArchivos.GuardarUsuario(nombre, contrasena, tipo);
        }

        // Restablecer SOLO la contraseña. No toca el historial de logs ni
        // el carnet: únicamente cambia el dato de la contraseña.
        public void RestablecerContrasena(string nombre, string tipo, string nuevaContrasena)
        {
            GestorArchivos.ActualizarContrasena(nombre, tipo, nuevaContrasena);
        }

        // Restablecer el USUARIO (o carnet) por completo. Este es el que
        // aclaraste que es una acción distinta y más "destructiva": borra
        // todo el contenido del archivo .txt correspondiente, no solo la
        // contraseña. Lo dejamos como un método separado justamente para
        // que en Program.cs nunca se pueda confundir un botón con el otro.
        public void RestablecerUsuario(string numeroCarnet)
        {
            GestorArchivos.BorrarContenidoCarnet(numeroCarnet);
        }
    }
}
