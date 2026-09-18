using System;

namespace Veterinaria.Logica
{
    // Clase ABSTRACTA: significa que nunca vas a poder hacer "new Usuario(...)"
    // directamente. Solo sirve como molde para que Recepcionista, Doctor y
    // Desarrollador hereden de ella. Esto tiene sentido porque en tu sistema
    // no existe un "usuario genérico" que entre al programa: siempre es
    // recepcionista, doctor o desarrollador.
    public abstract class Usuario
    {
        // Nombre es público porque muchas partes del programa necesitan
        // leerlo (por ejemplo, para saber a qué archivo de Logs escribirle:
        // "israel.txt", "recepcionista1.txt", etc.)
        public string Nombre { get; set; }

        // Contrasena es "protected": solo esta clase y sus hijas
        // (Recepcionista, Doctor, Desarrollador) pueden verla directamente.
        // Nadie fuera de la jerarquía de Usuario debería poder leer o
        // modificar la contraseña sin pasar por un método controlado.
        protected string Contrasena { get; set; }

        // Constructor: cada vez que se crea un Recepcionista, Doctor o
        // Desarrollador, por dentro se está llamando a este constructor
        // de Usuario primero (con la palabra clave "base(...)", que vas a
        // ver en las clases hijas).
        public Usuario(string nombre, string contrasena)
        {
            Nombre = nombre;
            Contrasena = contrasena;
        }

        // Método que usa el login para comparar la contraseña escrita
        // contra la guardada. Lo dejamos aquí (y no repetido en cada clase
        // hija) porque el proceso de "comparar contraseña" es idéntico sin
        // importar el tipo de usuario. Esto es reutilización de código,
        // una de las ventajas principales de la herencia.
        public bool ValidarContrasena(string contrasenaIngresada)
        {
            return Contrasena == contrasenaIngresada;
        }

        // Método "virtual": lo marcamos así porque cada tipo de usuario va
        // a registrar su sesión de forma un poco distinta (recuerda que
        // Israel no guarda contraseña en su log, pero los demás sí).
        // Las clases hijas van a poder "sobrescribirlo" (override) para
        // ajustar ese detalle sin romper el resto del código.
        public virtual string ObtenerNombreArchivoLog()
        {
            return Nombre.ToLower() + ".txt";
        }

        // Comportamiento POR DEFECTO para armar la línea que se escribe en
        // el log de sesión: nombre, contraseña, inicio, cierre y duración.
        // Esto es lo que usan Recepcionista y Doctor sin ningún cambio.
        // Desarrollador va a sobrescribir (override) este método, porque
        // Israel es el único caso especial que NO debe guardar su
        // contraseña en el log.
        public virtual string ObtenerLineaLog(DateTime inicio, DateTime cierre)
        {
            TimeSpan duracion = cierre - inicio;
            return "Nombre: " + Nombre +
                   " | Contrasena: " + Contrasena +
                   " | Inicio: " + inicio +
                   " | Cierre: " + cierre +
                   " | Duracion: " + (int)duracion.TotalMinutes + " min";
        }
    }
}
