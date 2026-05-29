// Program.cs
// Sistema Integral de Gestion de Biblioteca Universitaria
// Universidad Don Bosco - Programacion de Algoritmos
// Desafio Final Ciclo I - 2026
//
// Estudiante: Carlos Ernesto Melendez
// Carnet: ME210087
//
// Nota: Este sistema fue desarrollado usando structs, arreglos, archivos de texto,
// estructuras de decision y repeticion como se pidio en el requerimiento.

using System;

namespace BibliotecaUDB
{
    class Program
    {
        // arreglos globales para almacenar los datos en memoria
        // maximo: 10 libros, 5 usuarios, 10 prestamos
        static Libro[] libros = new Libro[10];
        static Usuario[] usuarios = new Usuario[5];
        static Prestamo[] prestamos = new Prestamo[10];

        // contadores de cuantos elementos hay actualmente
        static int totalLibros = 0;
        static int totalUsuarios = 0;
        static int totalPrestamos = 0;

        static void Main(string[] args)
        {
            // al iniciar el sistema, cargar los datos desde los archivos
            Console.WriteLine("Cargando datos...");
            totalLibros = ManejoArchivos.CargarLibros(libros);
            totalUsuarios = ManejoArchivos.CargarUsuarios(usuarios);
            totalPrestamos = ManejoArchivos.CargarPrestamos(prestamos);

            Console.WriteLine("Libros cargados: " + totalLibros);
            Console.WriteLine("Usuarios cargados: " + totalUsuarios);
            Console.WriteLine("Prestamos cargados: " + totalPrestamos);
            System.Threading.Thread.Sleep(1200); // pequeña pausa para que se vea el mensaje

            int opcion = 0;

            do
            {
                MostrarMenuPrincipal();

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
                        ModuloLibros.MostrarMenu(libros, ref totalLibros);
                        break;

                    case 2:
                        ModuloUsuarios.MostrarMenu(usuarios, ref totalUsuarios);
                        break;

                    case 3:
                        ModuloPrestamos.MostrarMenu(
                            prestamos, ref totalPrestamos,
                            libros, totalLibros,
                            usuarios, totalUsuarios);
                        break;

                    case 4:
                        GuardarManual();
                        break;

                    case 5:
                        Salir();
                        break;

                    default:
                        Console.WriteLine("\nOpcion no valida. Presione cualquier tecla para continuar.");
                        Console.ReadKey();
                        break;
                }

            } while (opcion != 5);
        }

        static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║     SISTEMA DE GESTION DE BIBLIOTECA         ║");
            Console.WriteLine("║         Universidad Don Bosco                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║                                              ║");
            Console.WriteLine("║   1. Gestion de Libros                       ║");
            Console.WriteLine("║   2. Gestion de Usuarios                     ║");
            Console.WriteLine("║   3. Gestion de Prestamos                    ║");
            Console.WriteLine("║   4. Guardar datos manualmente               ║");
            Console.WriteLine("║   5. Salir del Sistema                       ║");
            Console.WriteLine("║                                              ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");

            // mostrar resumen de datos cargados
            Console.WriteLine("║  Libros: {0,-5} Usuarios: {1,-4} Prestamos: {2,-4}   ║",
                totalLibros, totalUsuarios, totalPrestamos);

            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.Write("\n  Seleccione una opcion: ");
        }

        // guardar todos los datos manualmente
        static void GuardarManual()
        {
            Console.Clear();
            Console.WriteLine("Guardando datos...");
            ManejoArchivos.GuardarLibros(libros, totalLibros);
            ManejoArchivos.GuardarUsuarios(usuarios, totalUsuarios);
            ManejoArchivos.GuardarPrestamos(prestamos, totalPrestamos);
            Console.WriteLine("Datos guardados correctamente.");
            Console.ReadKey();
        }

        // al salir, guardar los datos automaticamente
        static void Salir()
        {
            Console.Clear();
            Console.Write("¿Esta seguro que desea salir? (s/n): ");
            string resp = Console.ReadLine().Trim().ToLower();

            if (resp == "s")
            {
                Console.WriteLine("\nGuardando datos antes de salir...");
                ManejoArchivos.GuardarLibros(libros, totalLibros);
                ManejoArchivos.GuardarUsuarios(usuarios, totalUsuarios);
                ManejoArchivos.GuardarPrestamos(prestamos, totalPrestamos);
                Console.WriteLine("Datos guardados. Hasta luego.");
                System.Threading.Thread.Sleep(900);
                Environment.Exit(0);
            }
        }
    }
}
