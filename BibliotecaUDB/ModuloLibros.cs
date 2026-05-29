// ModuloLibros.cs
// Modulo A - Gestion de Libros
// Aqui van todas las operaciones relacionadas con los libros

using System;

namespace BibliotecaUDB
{
    static class ModuloLibros
    {
        // mostrar el submenu de libros y procesar la opcion elegida
        public static void MostrarMenu(Libro[] libros, ref int totalLibros)
        {
            int opcion = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("================================================");
                Console.WriteLine("         MODULO A - GESTION DE LIBROS          ");
                Console.WriteLine("================================================");
                Console.WriteLine("  1. Registrar nuevo libro");
                Console.WriteLine("  2. Buscar libro por codigo");
                Console.WriteLine("  3. Listar todos los libros");
                Console.WriteLine("  4. Eliminar libro");
                Console.WriteLine("  0. Volver al menu principal");
                Console.WriteLine("================================================");
                Console.Write("  Seleccione una opcion: ");

                try
                {
                    opcion = int.Parse(Console.ReadLine());
                }
                catch
                {
                    opcion = -1; // opcion invalida
                }

                switch (opcion)
                {
                    case 1:
                        RegistrarLibro(libros, ref totalLibros);
                        break;
                    case 2:
                        BuscarLibro(libros, totalLibros);
                        break;
                    case 3:
                        ListarLibros(libros, totalLibros);
                        break;
                    case 4:
                        EliminarLibro(libros, ref totalLibros);
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("\n  Opcion no valida. Intente de nuevo.");
                        Console.ReadKey();
                        break;
                }

            } while (opcion != 0);
        }

        // registrar un nuevo libro en el arreglo
        static void RegistrarLibro(Libro[] libros, ref int totalLibros)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("           REGISTRAR NUEVO LIBRO               ");
            Console.WriteLine("================================================");

            // verificar que no se haya llegado al limite
            if (totalLibros >= libros.Length)
            {
                Console.WriteLine("\n  ERROR: Se alcanzo el limite maximo de libros (" + libros.Length + ").");
                Console.ReadKey();
                return;
            }

            Libro nuevo = new Libro();

            // codigo del libro
            bool codigoValido = false;
            while (!codigoValido)
            {
                Console.Write("\n  Codigo (8 caracteres alfanumericos, ej: LIB00001): ");
                nuevo.Codigo = Console.ReadLine().Trim().ToUpper();

                if (!Validaciones.ValidarCodigoLibro(nuevo.Codigo))
                {
                    Console.WriteLine("  Error: el codigo debe tener exactamente 8 caracteres alfanumericos.");
                }
                else if (BuscarIndicePorCodigo(libros, totalLibros, nuevo.Codigo) >= 0)
                {
                    Console.WriteLine("  Error: ya existe un libro con ese codigo.");
                }
                else
                {
                    codigoValido = true;
                }
            }

            // titulo
            do
            {
                Console.Write("  Titulo: ");
                nuevo.Titulo = Console.ReadLine().Trim();
                if (!Validaciones.NoEstaVacio(nuevo.Titulo))
                    Console.WriteLine("  Error: el titulo no puede estar vacio.");
            } while (!Validaciones.NoEstaVacio(nuevo.Titulo));

            // autor
            do
            {
                Console.Write("  Autor: ");
                nuevo.Autor = Console.ReadLine().Trim();
                if (!Validaciones.NoEstaVacio(nuevo.Autor))
                    Console.WriteLine("  Error: el autor no puede estar vacio.");
            } while (!Validaciones.NoEstaVacio(nuevo.Autor));

            // editorial
            do
            {
                Console.Write("  Editorial: ");
                nuevo.Editorial = Console.ReadLine().Trim();
                if (!Validaciones.NoEstaVacio(nuevo.Editorial))
                    Console.WriteLine("  Error: la editorial no puede estar vacia.");
            } while (!Validaciones.NoEstaVacio(nuevo.Editorial));

            // año de publicacion
            bool anioValido = false;
            while (!anioValido)
            {
                Console.Write("  Año de publicacion (1900-" + DateTime.Now.Year + "): ");
                try
                {
                    nuevo.AnioPublicacion = int.Parse(Console.ReadLine());
                    if (!Validaciones.ValidarAnio(nuevo.AnioPublicacion))
                        Console.WriteLine("  Error: el año debe estar entre 1900 y " + DateTime.Now.Year + ".");
                    else
                        anioValido = true;
                }
                catch
                {
                    Console.WriteLine("  Error: ingrese un año valido (solo numeros).");
                }
            }

            // categoria
            do
            {
                Console.Write("  Categoria (ej: Ciencias, Historia, Literatura...): ");
                nuevo.Categoria = Console.ReadLine().Trim();
                if (!Validaciones.NoEstaVacio(nuevo.Categoria))
                    Console.WriteLine("  Error: la categoria no puede estar vacia.");
            } while (!Validaciones.NoEstaVacio(nuevo.Categoria));

            // cantidad de ejemplares
            bool cantidadValida = false;
            while (!cantidadValida)
            {
                Console.Write("  Cantidad de ejemplares disponibles: ");
                try
                {
                    nuevo.EjemplaresDisponibles = int.Parse(Console.ReadLine());
                    if (!Validaciones.ValidarCantidad(nuevo.EjemplaresDisponibles))
                        Console.WriteLine("  Error: la cantidad no puede ser negativa.");
                    else
                        cantidadValida = true;
                }
                catch
                {
                    Console.WriteLine("  Error: ingrese un numero entero valido.");
                }
            }

            // agregar el libro al arreglo
            libros[totalLibros] = nuevo;
            totalLibros++;

            Console.WriteLine("\n  Libro registrado exitosamente.");
            Console.ReadKey();
        }

        // buscar un libro por su codigo y mostrarlo
        static void BuscarLibro(Libro[] libros, int totalLibros)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("             BUSCAR LIBRO POR CODIGO           ");
            Console.WriteLine("================================================");

            if (totalLibros == 0)
            {
                Console.WriteLine("\n  No hay libros registrados en el sistema.");
                Console.ReadKey();
                return;
            }

            Console.Write("\n  Ingrese el codigo del libro: ");
            string codigo = Console.ReadLine().Trim().ToUpper();

            int indice = BuscarIndicePorCodigo(libros, totalLibros, codigo);

            if (indice < 0)
            {
                Console.WriteLine("\n  No se encontro ningun libro con el codigo: " + codigo);
            }
            else
            {
                MostrarDetallesLibro(libros[indice]);
            }

            Console.ReadKey();
        }

        // mostrar todos los libros en una tabla
        public static void ListarLibros(Libro[] libros, int totalLibros)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("            LISTADO DE LIBROS                  ");
            Console.WriteLine("================================================");

            if (totalLibros == 0)
            {
                Console.WriteLine("\n  No hay libros registrados.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n  Total de libros: " + totalLibros + "/" + libros.Length);
            Console.WriteLine();
            Console.WriteLine("  {0,-10} {1,-30} {2,-20} {3,-6} {4,-5}",
                "Codigo", "Titulo", "Autor", "Anio", "Ejmp.");
            Console.WriteLine("  " + new string('-', 75));

            // recorrer con for para mostrar todos los libros
            for (int i = 0; i < totalLibros; i++)
            {
                // si el titulo es muy largo lo corto para que no se desborde
                string titulo = libros[i].Titulo;
                if (titulo.Length > 28)
                    titulo = titulo.Substring(0, 25) + "...";

                string autor = libros[i].Autor;
                if (autor.Length > 18)
                    autor = autor.Substring(0, 15) + "...";

                Console.WriteLine("  {0,-10} {1,-30} {2,-20} {3,-6} {4,-5}",
                    libros[i].Codigo,
                    titulo,
                    autor,
                    libros[i].AnioPublicacion,
                    libros[i].EjemplaresDisponibles);
            }

            Console.WriteLine();
            Console.ReadKey();
        }

        // eliminar un libro del arreglo
        static void EliminarLibro(Libro[] libros, ref int totalLibros)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("              ELIMINAR LIBRO                   ");
            Console.WriteLine("================================================");

            if (totalLibros == 0)
            {
                Console.WriteLine("\n  No hay libros registrados.");
                Console.ReadKey();
                return;
            }

            Console.Write("\n  Ingrese el codigo del libro a eliminar: ");
            string codigo = Console.ReadLine().Trim().ToUpper();

            int indice = BuscarIndicePorCodigo(libros, totalLibros, codigo);

            if (indice < 0)
            {
                Console.WriteLine("  No se encontro el libro con codigo: " + codigo);
                Console.ReadKey();
                return;
            }

            // mostrar datos del libro antes de confirmar
            MostrarDetallesLibro(libros[indice]);

            Console.Write("\n  ¿Esta seguro que desea eliminar este libro? (s/n): ");
            string confirmacion = Console.ReadLine().Trim().ToLower();

            if (confirmacion != "s")
            {
                Console.WriteLine("  Operacion cancelada.");
                Console.ReadKey();
                return;
            }

            // desplazar los elementos hacia la izquierda para llenar el espacio
            // esto lo vi en clase, es la forma de "eliminar" en arreglos
            for (int i = indice; i < totalLibros - 1; i++)
            {
                libros[i] = libros[i + 1];
            }

            // "limpiar" la ultima posicion
            libros[totalLibros - 1] = new Libro();
            totalLibros--;

            Console.WriteLine("  Libro eliminado correctamente.");
            Console.ReadKey();
        }

        // metodo auxiliar para mostrar los detalles de un libro
        static void MostrarDetallesLibro(Libro libro)
        {
            Console.WriteLine("\n  ----- Detalles del libro -----");
            Console.WriteLine("  Codigo:      " + libro.Codigo);
            Console.WriteLine("  Titulo:      " + libro.Titulo);
            Console.WriteLine("  Autor:       " + libro.Autor);
            Console.WriteLine("  Editorial:   " + libro.Editorial);
            Console.WriteLine("  Año:         " + libro.AnioPublicacion);
            Console.WriteLine("  Categoria:   " + libro.Categoria);
            Console.WriteLine("  Ejemplares:  " + libro.EjemplaresDisponibles);
        }

        // buscar el indice de un libro en el arreglo, retorna -1 si no existe
        public static int BuscarIndicePorCodigo(Libro[] libros, int total, string codigo)
        {
            for (int i = 0; i < total; i++)
            {
                if (libros[i].Codigo == codigo)
                    return i;
            }
            return -1;
        }
    }
}
