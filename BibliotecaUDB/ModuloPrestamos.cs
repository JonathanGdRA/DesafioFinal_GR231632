// ModuloPrestamos.cs
// Modulo C - Gestion de Prestamos
// Este fue el modulo mas dificil de hacer porque hay que validar varias cosas
// antes de registrar un prestamo

using System;

namespace BibliotecaUDB
{
    static class ModuloPrestamos
    {
        public static void MostrarMenu(
            Prestamo[] prestamos, ref int totalPrestamos,
            Libro[] libros, int totalLibros,
            Usuario[] usuarios, int totalUsuarios)
        {
            int opcion = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("================================================");
                Console.WriteLine("       MODULO C - GESTION DE PRESTAMOS         ");
                Console.WriteLine("================================================");
                Console.WriteLine("  1. Registrar nuevo prestamo");
                Console.WriteLine("  2. Registrar devolucion");
                Console.WriteLine("  3. Consultar historial de un usuario");
                Console.WriteLine("  4. Listar prestamos activos");
                Console.WriteLine("  5. Actualizar estado de prestamo");
                Console.WriteLine("  0. Volver al menu principal");
                Console.WriteLine("================================================");
                Console.Write("  Seleccione una opcion: ");

                try
                {
                    opcion = int.Parse(Console.ReadLine());
                }
                catch
                {
                    opcion = -1;
                }

                switch (opcion)
                {
                    case 1:
                        RegistrarPrestamo(prestamos, ref totalPrestamos,
                                          libros, totalLibros,
                                          usuarios, totalUsuarios);
                        break;
                    case 2:
                        RegistrarDevolucion(prestamos, totalPrestamos, libros, totalLibros);
                        break;
                    case 3:
                        HistorialUsuario(prestamos, totalPrestamos, libros, totalLibros);
                        break;
                    case 4:
                        ListarActivos(prestamos, totalPrestamos);
                        break;
                    case 5:
                        ActualizarEstado(prestamos, totalPrestamos);
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("\n  Opcion no valida.");
                        Console.ReadKey();
                        break;
                }

            } while (opcion != 0);
        }

        static void RegistrarPrestamo(
            Prestamo[] prestamos, ref int totalPrestamos,
            Libro[] libros, int totalLibros,
            Usuario[] usuarios, int totalUsuarios)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("          REGISTRAR NUEVO PRESTAMO             ");
            Console.WriteLine("================================================");

            if (totalPrestamos >= prestamos.Length)
            {
                Console.WriteLine("\n  ERROR: Se alcanzo el limite de prestamos (" + prestamos.Length + ").");
                Console.ReadKey();
                return;
            }

            Prestamo nuevo = new Prestamo();

            // buscar el usuario
            int indiceUsuario = -1;
            while (indiceUsuario < 0)
            {
                Console.Write("\n  Carne del usuario: ");
                string carne = Console.ReadLine().Trim();

                indiceUsuario = ModuloUsuarios.BuscarIndicePorCarne(usuarios, totalUsuarios, carne);

                if (indiceUsuario < 0)
                {
                    Console.WriteLine("  No existe un usuario con ese carne. Intente de nuevo.");
                }
                else if (usuarios[indiceUsuario].Estado == "inactivo")
                {
                    Console.WriteLine("  El usuario esta inactivo y no puede hacer prestamos.");
                    indiceUsuario = -1; // volver a pedir
                }
                else
                {
                    nuevo.CarneUsuario = usuarios[indiceUsuario].Carne;
                    Console.WriteLine("  Usuario: " + usuarios[indiceUsuario].NombreCompleto);
                }
            }

            // buscar el libro
            int indiceLibro = -1;
            while (indiceLibro < 0)
            {
                Console.Write("  Codigo del libro: ");
                string codigoLibro = Console.ReadLine().Trim().ToUpper();

                indiceLibro = ModuloLibros.BuscarIndicePorCodigo(libros, totalLibros, codigoLibro);

                if (indiceLibro < 0)
                {
                    Console.WriteLine("  No existe un libro con ese codigo. Intente de nuevo.");
                }
                else if (libros[indiceLibro].EjemplaresDisponibles <= 0)
                {
                    // no hay ejemplares disponibles
                    Console.WriteLine("  El libro \"" + libros[indiceLibro].Titulo + "\" no tiene ejemplares disponibles.");
                    indiceLibro = -1;
                }
                else
                {
                    nuevo.CodigoLibro = libros[indiceLibro].Codigo;
                    Console.WriteLine("  Libro: " + libros[indiceLibro].Titulo);
                    Console.WriteLine("  Ejemplares disponibles: " + libros[indiceLibro].EjemplaresDisponibles);
                }
            }

            // fecha de prestamo
            bool fechaValida = false;
            while (!fechaValida)
            {
                Console.Write("  Fecha de prestamo (dd/mm/yyyy): ");
                nuevo.FechaPrestamo = Console.ReadLine().Trim();

                if (!Validaciones.ValidarFecha(nuevo.FechaPrestamo))
                    Console.WriteLine("  Error: formato de fecha incorrecto. Use dd/mm/yyyy.");
                else
                    fechaValida = true;
            }

            // fecha estimada de devolucion
            fechaValida = false;
            while (!fechaValida)
            {
                Console.Write("  Fecha estimada de devolucion (dd/mm/yyyy): ");
                nuevo.FechaDevolucion = Console.ReadLine().Trim();

                if (!Validaciones.ValidarFecha(nuevo.FechaDevolucion))
                    Console.WriteLine("  Error: formato de fecha incorrecto. Use dd/mm/yyyy.");
                else
                    fechaValida = true;
            }

            // generar ID del prestamo usando la cantidad actual + 1
            nuevo.IdPrestamo = "PRE" + (totalPrestamos + 1).ToString("D5");
            nuevo.Estado = "activo";

            // reducir ejemplares disponibles del libro
            libros[indiceLibro].EjemplaresDisponibles--;

            prestamos[totalPrestamos] = nuevo;
            totalPrestamos++;

            Console.WriteLine("\n  Prestamo registrado. ID: " + nuevo.IdPrestamo);
            Console.WriteLine("  Ejemplares restantes del libro: " + libros[indiceLibro].EjemplaresDisponibles);
            Console.ReadKey();
        }

        static void RegistrarDevolucion(Prestamo[] prestamos, int totalPrestamos,
                                         Libro[] libros, int totalLibros)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("           REGISTRAR DEVOLUCION                ");
            Console.WriteLine("================================================");

            Console.Write("\n  Ingrese el ID del prestamo: ");
            string idPrestamo = Console.ReadLine().Trim().ToUpper();

            int indice = BuscarPorId(prestamos, totalPrestamos, idPrestamo);

            if (indice < 0)
            {
                Console.WriteLine("  No se encontro el prestamo con ID: " + idPrestamo);
                Console.ReadKey();
                return;
            }

            if (prestamos[indice].Estado == "devuelto")
            {
                Console.WriteLine("  Este prestamo ya fue devuelto anteriormente.");
                Console.ReadKey();
                return;
            }

            // mostrar datos del prestamo
            Console.WriteLine("\n  Carne usuario: " + prestamos[indice].CarneUsuario);
            Console.WriteLine("  Codigo libro:  " + prestamos[indice].CodigoLibro);
            Console.WriteLine("  Fecha prestamo: " + prestamos[indice].FechaPrestamo);
            Console.WriteLine("  Fecha limite:   " + prestamos[indice].FechaDevolucion);

            Console.Write("\n  ¿Confirmar devolucion? (s/n): ");
            string conf = Console.ReadLine().Trim().ToLower();

            if (conf != "s")
            {
                Console.WriteLine("  Operacion cancelada.");
                Console.ReadKey();
                return;
            }

            // actualizar estado del prestamo
            prestamos[indice].Estado = "devuelto";

            // incrementar los ejemplares del libro de vuelta
            int indiceLibro = ModuloLibros.BuscarIndicePorCodigo(
                libros, totalLibros, prestamos[indice].CodigoLibro);

            if (indiceLibro >= 0)
            {
                libros[indiceLibro].EjemplaresDisponibles++;
                Console.WriteLine("  Ejemplares del libro actualizados: " +
                    libros[indiceLibro].EjemplaresDisponibles);
            }

            Console.WriteLine("  Devolucion registrada exitosamente.");
            Console.ReadKey();
        }

        static void HistorialUsuario(Prestamo[] prestamos, int totalPrestamos,
                                      Libro[] libros, int totalLibros)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("         HISTORIAL DE PRESTAMOS DE USUARIO     ");
            Console.WriteLine("================================================");

            Console.Write("\n  Ingrese el carne del usuario: ");
            string carne = Console.ReadLine().Trim();

            int encontrados = 0;

            Console.WriteLine("\n  {0,-10} {1,-12} {2,-12} {3,-12} {4,-10}",
                "ID", "Codigo Libro", "F.Prestamo", "F.Devolucion", "Estado");
            Console.WriteLine("  " + new string('-', 60));

            // recorrer todos los prestamos del usuario
            for (int i = 0; i < totalPrestamos; i++)
            {
                if (prestamos[i].CarneUsuario == carne)
                {
                    Console.WriteLine("  {0,-10} {1,-12} {2,-12} {3,-12} {4,-10}",
                        prestamos[i].IdPrestamo,
                        prestamos[i].CodigoLibro,
                        prestamos[i].FechaPrestamo,
                        prestamos[i].FechaDevolucion,
                        prestamos[i].Estado);
                    encontrados++;
                }
            }

            if (encontrados == 0)
                Console.WriteLine("  No hay prestamos registrados para ese carne.");
            else
                Console.WriteLine("\n  Total de prestamos: " + encontrados);

            Console.ReadKey();
        }

        static void ListarActivos(Prestamo[] prestamos, int totalPrestamos)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("            PRESTAMOS ACTIVOS                  ");
            Console.WriteLine("================================================");

            int activos = 0;

            Console.WriteLine("\n  {0,-10} {1,-12} {2,-12} {3,-12}",
                "ID", "Carne", "Codigo Libro", "F.Devolucion");
            Console.WriteLine("  " + new string('-', 50));

            for (int i = 0; i < totalPrestamos; i++)
            {
                if (prestamos[i].Estado == "activo")
                {
                    Console.WriteLine("  {0,-10} {1,-12} {2,-12} {3,-12}",
                        prestamos[i].IdPrestamo,
                        prestamos[i].CarneUsuario,
                        prestamos[i].CodigoLibro,
                        prestamos[i].FechaDevolucion);
                    activos++;
                }
            }

            if (activos == 0)
                Console.WriteLine("  No hay prestamos activos en este momento.");
            else
                Console.WriteLine("\n  Total activos: " + activos);

            Console.ReadKey();
        }

        static void ActualizarEstado(Prestamo[] prestamos, int totalPrestamos)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("         ACTUALIZAR ESTADO DE PRESTAMO         ");
            Console.WriteLine("================================================");

            Console.Write("\n  Ingrese el ID del prestamo: ");
            string id = Console.ReadLine().Trim().ToUpper();

            int indice = BuscarPorId(prestamos, totalPrestamos, id);

            if (indice < 0)
            {
                Console.WriteLine("  Prestamo no encontrado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n  Estado actual: " + prestamos[indice].Estado);
            Console.WriteLine("  Opciones: activo / devuelto / vencido");
            Console.Write("  Nuevo estado: ");
            string nuevoEstado = Console.ReadLine().Trim().ToLower();

            // validar que sea uno de los estados validos
            if (nuevoEstado != "activo" && nuevoEstado != "devuelto" && nuevoEstado != "vencido")
            {
                Console.WriteLine("  Estado no valido. Use: activo, devuelto o vencido.");
            }
            else
            {
                prestamos[indice].Estado = nuevoEstado;
                Console.WriteLine("  Estado actualizado a: " + nuevoEstado);
            }

            Console.ReadKey();
        }

        // buscar prestamo por su ID
        static int BuscarPorId(Prestamo[] prestamos, int total, string id)
        {
            for (int i = 0; i < total; i++)
            {
                if (prestamos[i].IdPrestamo == id)
                    return i;
            }
            return -1;
        }
    }
}
