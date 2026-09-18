using System;
using System.Collections.Generic;
using Veterinaria.Logica;

namespace Veterinaria
{
    class Program
    {
        // ================================================================
        // PUNTO DE ENTRADA
        // ================================================================
        // Todo el programa arranca aquí. Esta es la única parte que C#
        // ejecuta automáticamente al iniciar; todo lo demás (Recepcionista,
        // Doctor, GestorArchivos, etc.) solo se usa porque ESTE método las
        // llama, directa o indirectamente.
        static void Main(string[] args)
        {
            bool salirDelPrograma = false;

            while (!salirDelPrograma)
            {
                MostrarMenuPrincipal();

                // intercept: true evita que la tecla se imprima sola en
                // pantalla, y nos deja revisar los "Modifiers" (si Ctrl,
                // Alt o Shift estaban presionados al mismo tiempo).
                ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

                // --- Combinación oculta del modo 3 ---
                // Nota importante: la tecla Windows real NO se puede leer
                // de forma confiable desde una aplicación de consola,
                // porque Windows la intercepta antes de que le llegue a
                // cualquier programa (Win+8, por ejemplo, ya está
                // reservada por el sistema operativo para abrir la app
                // anclada en la posición 8 de la barra de tareas). Por eso
                // usamos Ctrl+Alt+D como la combinación oculta: hace
                // exactamente el mismo papel (nadie la encuentra por
                // accidente) pero SÍ se puede capturar de forma segura
                // dentro de un programa de consola.
                bool controlPresionado = (tecla.Modifiers & ConsoleModifiers.Control) != 0;
                bool altPresionado = (tecla.Modifiers & ConsoleModifiers.Alt) != 0;

                if (controlPresionado && altPresionado && tecla.Key == ConsoleKey.D)
                {
                    EjecutarModoDesarrollador();
                    continue;
                }

                if (tecla.KeyChar == '1')
                {
                    EjecutarModoRecepcion();
                }
                else if (tecla.KeyChar == '2')
                {
                    EjecutarModoDoctor();
                }
                else if (tecla.KeyChar == '3')
                {
                    salirDelPrograma = true;
                }
            }
        }

        static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("       SISTEMA DE VETERINARIA");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Recepcion");
            Console.WriteLine("2. Doctor");
            Console.WriteLine("3. Salir");
            Console.Write("Elige una opcion: ");
        }

        // Ayudante compartido por los tres modos: lee la contraseña
        // mostrando "*" en vez del caracter real, tecla por tecla.
        static string LeerContrasenaOculta()
        {
            string contrasena = "";
            ConsoleKeyInfo tecla;

            do
            {
                tecla = Console.ReadKey(intercept: true);

                if (tecla.Key == ConsoleKey.Backspace && contrasena.Length > 0)
                {
                    contrasena = contrasena.Substring(0, contrasena.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(tecla.KeyChar))
                {
                    contrasena += tecla.KeyChar;
                    Console.Write("*");
                }
            }
            while (tecla.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return contrasena;
        }

        // ================================================================
        // MODO 1: RECEPCION
        // ================================================================

        static void EjecutarModoRecepcion()
        {
            Console.Clear();
            Console.WriteLine("=== INGRESO RECEPCION ===");
            Console.Write("Usuario: ");
            string nombre = Console.ReadLine();
            Console.Write("Contrasena: ");
            string contrasena = LeerContrasenaOculta();

            Recepcionista recepcionista = null;

            // Recorremos la lista plana que nos da GestorArchivos y
            // buscamos una línea cuyo nombre, contraseña y tipo coincidan.
            // datos[0] = nombre, datos[1] = contrasena, datos[2] = tipo.
            foreach (string[] datos in GestorArchivos.LeerUsuarios())
            {
                if (datos[0] == nombre && datos[1] == contrasena && datos[2] == "Recepcionista")
                {
                    recepcionista = new Recepcionista(datos[0], datos[1]);
                }
            }

            if (recepcionista == null)
            {
                Console.WriteLine("Usuario o contrasena incorrectos.");
                Console.WriteLine("Presiona una tecla para volver...");
                Console.ReadKey();
                return;
            }

            // A partir de aquí ya sabemos que el login fue exitoso, así
            // que arrancamos su registro de sesión.
            RegistroSesion sesion = new RegistroSesion(recepcionista);
            sesion.IniciarSesion();

            MenuRecepcion(recepcionista);

            // Cuando MenuRecepcion termina (la persona eligió "Cerrar
            // sesion"), cerramos el registro. Esto es lo que dispara,
            // adentro de RegistroSesion, el guardado en Logs.
            sesion.CerrarSesion();
        }

        static void MenuRecepcion(Recepcionista recepcionista)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== MODO RECEPCION (" + recepcionista.Nombre + ") ===");
                Console.WriteLine("1. Ingresar numero de carnet");
                Console.WriteLine("2. Cerrar sesion");
                Console.Write("Elige una opcion: ");
                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    TrabajarConCarnetRecepcion(recepcionista);
                }
                else if (opcion == "2")
                {
                    salir = true;
                }
            }
        }

        // Todo lo que la recepcionista puede hacer UNA VEZ que ya escribió
        // un número de carnet. Lo separamos de MenuRecepcion para que ese
        // método de arriba se quede simple y fácil de leer.
        static void TrabajarConCarnetRecepcion(Recepcionista recepcionista)
        {
            Console.Write("Numero de carnet: ");
            string numeroCarnet = Console.ReadLine();

            Carnet carnet = recepcionista.BuscarCarnet(numeroCarnet);

            if (carnet == null)
            {
                Console.WriteLine("Ese carnet no existe.");
                Console.Write("¿Deseas crearlo? (S/N): ");
                string respuesta = Console.ReadLine();

                if (respuesta.ToUpper() == "S")
                {
                    CrearCarnetNuevo(recepcionista, numeroCarnet);
                }

                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("Carnet: " + carnet.NumeroCarnet + " - " + carnet.NombreAnimal + " (" + carnet.Especie + ")");
                Console.WriteLine("1. Ver citas y receta");
                Console.WriteLine("2. Agendar nueva cita");
                Console.WriteLine("3. Cambiar dia/horario de una cita");
                Console.WriteLine("4. Volver al menu");
                Console.Write("Elige una opcion: ");
                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    MostrarCitasYReceta(recepcionista, carnet);
                    Console.ReadKey();
                }
                else if (opcion == "2")
                {
                    AgendarCitaDesdeMenu(recepcionista, carnet);
                }
                else if (opcion == "3")
                {
                    CambiarCitaDesdeMenu(recepcionista, carnet);
                }
                else if (opcion == "4")
                {
                    salir = true;
                }
            }
        }

        static void CrearCarnetNuevo(Recepcionista recepcionista, string numeroCarnet)
        {
            Console.Write("Nombre del animal: ");
            string nombreAnimal = Console.ReadLine();
            Console.Write("Especie: ");
            string especie = Console.ReadLine();
            Console.Write("DPI del dueno: ");
            string dpi = Console.ReadLine();
            Console.Write("Celular: ");
            string celular = Console.ReadLine();
            Console.Write("Direccion: ");
            string direccion = Console.ReadLine();

            recepcionista.CrearCarnet(numeroCarnet, nombreAnimal, especie, dpi, celular, direccion);
            Console.WriteLine("Carnet creado correctamente.");
        }

        static void MostrarCitasYReceta(Recepcionista recepcionista, Carnet carnet)
        {
            if (carnet.Citas.Count == 0)
            {
                Console.WriteLine("Este carnet todavia no tiene citas.");
                return;
            }

            for (int i = 0; i < carnet.Citas.Count; i++)
            {
                Cita cita = carnet.Citas[i];
                Console.WriteLine((i + 1) + ". " + cita.Fecha.ToShortDateString() +
                                   " " + cita.Hora +
                                   (cita.Atendida ? " (Atendida)" : " (Pendiente)"));

                // Recepcion SOLO puede ver la receta, nunca el reporte
                // completo de procedimientos. Por eso aquí llamamos a
                // VerReceta() y nunca a VerReporte() (ese método ni
                // siquiera existe en la clase Recepcionista).
                Console.WriteLine("   Receta: " + recepcionista.VerReceta(cita));
            }
        }

        static void AgendarCitaDesdeMenu(Recepcionista recepcionista, Carnet carnet)
        {
            Console.Write("Fecha (yyyy-MM-dd): ");
            DateTime fecha = DateTime.Parse(Console.ReadLine());
            Console.Write("Hora (ej. 10:00): ");
            string hora = Console.ReadLine();

            recepcionista.AgendarCita(carnet, fecha, hora);
            Console.WriteLine("Cita agendada correctamente.");
            Console.ReadKey();
        }

        static void CambiarCitaDesdeMenu(Recepcionista recepcionista, Carnet carnet)
        {
            MostrarCitasYReceta(recepcionista, carnet);

            Console.Write("¿Cual cita quieres cambiar? (numero de la lista): ");
            int indice = int.Parse(Console.ReadLine()) - 1;

            if (indice < 0 || indice >= carnet.Citas.Count)
            {
                Console.WriteLine("Numero invalido.");
                Console.ReadKey();
                return;
            }

            Cita citaSeleccionada = carnet.Citas[indice];

            Console.Write("Nueva fecha (yyyy-MM-dd): ");
            DateTime nuevaFecha = DateTime.Parse(Console.ReadLine());
            Console.Write("Nueva hora: ");
            string nuevaHora = Console.ReadLine();

            recepcionista.CambiarCita(carnet, citaSeleccionada, nuevaFecha, nuevaHora);
            Console.WriteLine("Cita actualizada.");
            Console.ReadKey();
        }

        // ================================================================
        // MODO 2: DOCTOR
        // ================================================================

        static void EjecutarModoDoctor()
        {
            Console.Clear();
            Console.WriteLine("=== INGRESO DOCTOR ===");
            Console.Write("Usuario: ");
            string nombre = Console.ReadLine();
            Console.Write("Contrasena: ");
            string contrasena = LeerContrasenaOculta();

            Doctor doctor = null;

            foreach (string[] datos in GestorArchivos.LeerUsuarios())
            {
                if (datos[0] == nombre && datos[1] == contrasena && datos[2] == "Doctor")
                {
                    doctor = new Doctor(datos[0], datos[1]);
                }
            }

            if (doctor == null)
            {
                Console.WriteLine("Usuario o contrasena incorrectos.");
                Console.WriteLine("Presiona una tecla para volver...");
                Console.ReadKey();
                return;
            }

            RegistroSesion sesion = new RegistroSesion(doctor);
            sesion.IniciarSesion();

            MenuDoctor(doctor);

            sesion.CerrarSesion();
        }

        static void MenuDoctor(Doctor doctor)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== MODO DOCTOR (" + doctor.Nombre + ") ===");
                Console.WriteLine("1. Ingresar numero de carnet");
                Console.WriteLine("2. Cerrar sesion");
                Console.Write("Elige una opcion: ");
                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    TrabajarConCarnetDoctor(doctor);
                }
                else if (opcion == "2")
                {
                    salir = true;
                }
            }
        }

        static void TrabajarConCarnetDoctor(Doctor doctor)
        {
            Console.Write("Numero de carnet: ");
            string numeroCarnet = Console.ReadLine();

            Carnet carnet = doctor.BuscarCarnet(numeroCarnet);

            if (carnet == null)
            {
                Console.WriteLine("Ese carnet no existe.");
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
                return;
            }

            // Aquí es donde se resuelve justo lo que pediste al inicio:
            // si hay una cita programada para HOY, se la proponemos
            // directamente; si no, dejamos que el doctor elija otra de la
            // lista manualmente.
            Cita citaActual = doctor.BuscarCitaDeHoy(carnet);

            if (citaActual != null)
            {
                Console.WriteLine("Hay una cita programada para hoy a las " + citaActual.Hora + ".");
                Console.Write("¿Es esta la cita? (S/N): ");
                string respuesta = Console.ReadLine();

                if (respuesta.ToUpper() != "S")
                {
                    citaActual = ElegirCitaManualmente(carnet);
                }
            }
            else
            {
                Console.WriteLine("No hay ninguna cita programada para hoy en este carnet.");
                citaActual = ElegirCitaManualmente(carnet);
            }

            if (citaActual == null)
            {
                Console.WriteLine("No se selecciono ninguna cita.");
                Console.ReadKey();
                return;
            }

            MenuAtenderCita(doctor, carnet, citaActual);
        }

        // La opcion de "buscar otra" que pediste: lista TODAS las citas del
        // carnet (no solo las de hoy) para que el doctor elija a mano.
        static Cita ElegirCitaManualmente(Carnet carnet)
        {
            if (carnet.Citas.Count == 0)
            {
                Console.WriteLine("Este carnet no tiene ninguna cita registrada.");
                return null;
            }

            for (int i = 0; i < carnet.Citas.Count; i++)
            {
                Cita cita = carnet.Citas[i];
                Console.WriteLine((i + 1) + ". " + cita.Fecha.ToShortDateString() +
                                   " " + cita.Hora +
                                   (cita.Atendida ? " (Atendida)" : " (Pendiente)"));
            }

            Console.Write("Elige el numero de la cita: ");
            int indice = int.Parse(Console.ReadLine()) - 1;

            if (indice < 0 || indice >= carnet.Citas.Count)
            {
                return null;
            }

            return carnet.Citas[indice];
        }

        static void MenuAtenderCita(Doctor doctor, Carnet carnet, Cita cita)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("Cita del " + cita.Fecha.ToShortDateString() + " " + cita.Hora);
                Console.WriteLine("1. Ver reporte completo (fecha, receta, procedimientos)");
                Console.WriteLine("2. Registrar procedimiento realizado");
                Console.WriteLine("3. Extender receta");
                Console.WriteLine("4. Finalizar cita");
                Console.WriteLine("5. Volver al menu");
                Console.Write("Elige una opcion: ");
                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    // El doctor SÍ puede ver el reporte completo, a
                    // diferencia de Recepcionista. Por eso este es el
                    // único lugar del programa donde llamamos VerReporte().
                    Console.WriteLine(doctor.VerReporte(cita));
                    Console.ReadKey();
                }
                else if (opcion == "2")
                {
                    Console.Write("Descripcion del procedimiento: ");
                    string descripcion = Console.ReadLine();
                    doctor.RegistrarProcedimiento(cita, descripcion);
                    GestorArchivos.GuardarCarnet(carnet);
                    Console.WriteLine("Procedimiento registrado.");
                    Console.ReadKey();
                }
                else if (opcion == "3")
                {
                    Console.Write("Medicamento: ");
                    string medicamento = Console.ReadLine();
                    Console.Write("Dosis: ");
                    string dosis = Console.ReadLine();
                    Console.Write("Indicaciones: ");
                    string indicaciones = Console.ReadLine();
                    doctor.ExtenderReceta(cita, medicamento, dosis, indicaciones);
                    GestorArchivos.GuardarCarnet(carnet);
                    Console.WriteLine("Receta extendida.");
                    Console.ReadKey();
                }
                else if (opcion == "4")
                {
                    doctor.FinalizarCita(carnet, cita);
                    Console.WriteLine("Cita finalizada.");
                    Console.ReadKey();
                    salir = true;
                }
                else if (opcion == "5")
                {
                    salir = true;
                }
            }
        }

        // ================================================================
        // MODO 3: DESARROLLADOR (oculto)
        // ================================================================

        static void EjecutarModoDesarrollador()
        {
            Console.Clear();
            Console.WriteLine("=== MODO DESARROLLADOR ===");
            Console.Write("Usuario: ");
            string nombre = Console.ReadLine();
            Console.Write("Contrasena: ");
            string contrasena = LeerContrasenaOculta();

            Desarrollador desarrollador = ValidarLoginDesarrollador(nombre, contrasena);

            if (desarrollador == null)
            {
                Console.WriteLine("Usuario o contrasena incorrectos.");
                Console.WriteLine("Presiona una tecla para volver...");
                Console.ReadKey();
                return;
            }

            RegistroSesion sesion = new RegistroSesion(desarrollador);
            sesion.IniciarSesion();

            MenuDesarrollador(desarrollador);

            sesion.CerrarSesion();
        }

        static Desarrollador ValidarLoginDesarrollador(string nombre, string contrasena)
        {
            // Israel es el desarrollador "raiz": es el unico que existe
            // desde el principio, sin que nadie lo haya creado antes,
            // asi que sus credenciales estan fijas aqui mismo en el
            // codigo (no hay ningun archivo del que leerlas la primera
            // vez que arranca el sistema).
            if (nombre == "Israel" && contrasena == "822007")
            {
                return new Desarrollador(nombre, contrasena, esRaiz: true);
            }

            // Cualquier otro desarrollador SI fue creado por Israel en
            // algun momento, asi que su nombre y contrasena ya estan
            // guardados en desarrolladores.txt.
            foreach (string[] datos in GestorArchivos.LeerDesarrolladores())
            {
                if (datos[0] == nombre && datos[1] == contrasena)
                {
                    return new Desarrollador(nombre, contrasena, esRaiz: false);
                }
            }

            return null;
        }

        static void MenuDesarrollador(Desarrollador desarrollador)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== MODO DESARROLLADOR (" + desarrollador.Nombre + ") ===");
                Console.WriteLine("1. Crear usuario (Recepcionista o Doctor)");
                Console.WriteLine("2. Restablecer contrasena de un usuario");
                Console.WriteLine("3. Restablecer usuario (borra TODO su historial)");
                Console.WriteLine("4. Crear nuevo desarrollador");
                Console.WriteLine("5. Salir");
                Console.Write("Elige una opcion: ");
                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    Console.Write("Nombre del nuevo usuario: ");
                    string nombre = Console.ReadLine();
                    Console.Write("Contrasena: ");
                    string contrasena = Console.ReadLine();
                    Console.Write("Tipo (Recepcionista/Doctor): ");
                    string tipo = Console.ReadLine();

                    desarrollador.CrearUsuario(nombre, contrasena, tipo);
                    Console.WriteLine("Usuario creado.");
                    Console.ReadKey();
                }
                else if (opcion == "2")
                {
                    Console.Write("Nombre del usuario: ");
                    string nombre = Console.ReadLine();
                    Console.Write("Tipo (Recepcionista/Doctor): ");
                    string tipo = Console.ReadLine();
                    Console.Write("Nueva contrasena: ");
                    string nuevaContrasena = Console.ReadLine();

                    desarrollador.RestablecerContrasena(nombre, tipo, nuevaContrasena);
                    Console.WriteLine("Contrasena actualizada.");
                    Console.ReadKey();
                }
                else if (opcion == "3")
                {
                    Console.Write("Numero de carnet a restablecer: ");
                    string numeroCarnet = Console.ReadLine();

                    // Confirmacion obligatoria, porque esta accion borra
                    // TODO el contenido del archivo, a diferencia de solo
                    // restablecer la contrasena.
                    Console.Write("Esto borrara TODO el historial de ese carnet. ¿Confirmas? (S/N): ");
                    string confirmacion = Console.ReadLine();

                    if (confirmacion.ToUpper() == "S")
                    {
                        desarrollador.RestablecerUsuario(numeroCarnet);
                        Console.WriteLine("Carnet restablecido.");
                    }
                    else
                    {
                        Console.WriteLine("Operacion cancelada.");
                    }
                    Console.ReadKey();
                }
                else if (opcion == "4")
                {
                    // CrearDesarrollador ya trae la validacion de EsRaiz
                    // adentro de la propia clase Desarrollador. Aqui solo
                    // necesitamos atrapar la excepcion si alguien que no
                    // es Israel llega a este punto (por ejemplo, si en el
                    // futuro agregas otra forma de entrar a este menu).
                    try
                    {
                        Console.Write("Nombre del nuevo desarrollador: ");
                        string nombre = Console.ReadLine();
                        Console.Write("Contrasena: ");
                        string contrasena = Console.ReadLine();

                        desarrollador.CrearDesarrollador(nombre, contrasena);
                        Console.WriteLine("Desarrollador creado.");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("No se pudo crear: " + ex.Message);
                    }
                    Console.ReadKey();
                }
                else if (opcion == "5")
                {
                    salir = true;
                }
            }
        }
    }
}
