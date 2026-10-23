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

        // AHORA SÍ es un override real del método virtual que dejamos en
        // Usuario. La palabra "override" es lo que activa el polimorfismo:
        // cuando el programa tenga un Desarrollador y llame a
        // ObtenerLineaLog(...), automáticamente va a usar ESTA versión y
        // no la de Usuario, sin que el código que lo llama tenga que saber
        // que es un caso especial.
        public override string ObtenerLineaLog(DateTime inicio, DateTime cierre)
        {
            if (EsRaiz)
            {
                TimeSpan duracion = cierre - inicio;
                return "Nombre: " + Nombre +
                       " | Inicio: " + inicio +
                       " | Cierre: " + cierre +
                       " | Duracion: " + (int)duracion.TotalMinutes + " min";
            }

            // Los demás desarrolladores (los que Israel cree) SÍ guardan su
            // contraseña, igual que cualquier Recepcionista o Doctor. En
            // vez de repetir ese mismo texto aquí, reutilizamos el
            // comportamiento del padre con "base.ObtenerLineaLog(...)".
            return base.ObtenerLineaLog(inicio, cierre);
        }

        // Solo Israel puede crear nuevos desarrolladores. En vez de confiar
        // en que Program.cs recuerde validar esto antes de llamar al
        // método, la regla se pone AQUÍ, adentro de la propia clase. Así,
        // sin importar desde dónde se llame este método en el futuro, la
        // regla de negocio no se puede saltar.
        //
        // Nota: le pasamos "contrasena" directo a GestorArchivos en vez de
        // pasarle el objeto "nuevo" completo, porque Contrasena es
        // protected — ni siquiera GestorArchivos podría leerla desde un
        // objeto Desarrollador. Pasar el dato ya "suelto" evita tener que
        // debilitar esa protección.
        public Desarrollador CrearDesarrollador(string nombre, string contrasena)
        {
            if (!EsRaiz)
            {
                throw new InvalidOperationException(
                    "Solo el desarrollador raiz (Israel) puede crear nuevos desarrolladores.");
            }

            Desarrollador nuevo = new Desarrollador(nombre, contrasena, false);
            GestorArchivos.GuardarDesarrollador(nombre, contrasena);
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
