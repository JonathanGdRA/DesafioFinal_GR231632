using System;
using System.IO;

struct Libro
{
    public string Codigo;
    public string Titulo;
    public string Autor;
    public string Editorial;
    public int Anio;
    public string Categoria;
    public int Cantidad;
}

struct Usuario
{
    public string Carne;
    public string Nombre;
    public string Carrera;
    public string Correo;
    public string Telefono;
    public string Estado;
}

struct Prestamo
{
    public string CodigoLibro;
    public string CarneUsuario;
    public string FechaPrestamo;
    public string Estado;
}

class Program
{
    static Libro[] libros = new Libro[10];
    static Usuario[] usuarios = new Usuario[5];
    static Prestamo[] prestamos = new Prestamo[10];

    static int totalLibros = 0;
    static int totalUsuarios = 0;
    static int totalPrestamos = 0;

    // MATRIZ
    static string[,] categorias = new string[3, 2]
    {
        {"1", "Programación"},
        {"2", "Base de Datos"},
        {"3", "Redes"}
    };

    static void Main()
    {
        CargarLibros();
        CargarUsuarios();
        CargarPrestamos();

        int opcion = 0;

        do
        {
            Console.Clear();
            Console.WriteLine("===== SISTEMA DE BIBLIOTECA =====");
            Console.WriteLine("1. Gestión de Libros");
            Console.WriteLine("2. Gestión de Usuarios");
            Console.WriteLine("3. Gestión de Préstamos");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");

            int.TryParse(Console.ReadLine(), out opcion);

            switch (opcion)
            {
                case 1:
                    MenuLibros();
                    break;

                case 2:
                    MenuUsuarios();
                    break;

                case 3:
                    MenuPrestamos();
                    break;

                case 4:
                    Console.WriteLine("Saliendo...");
                    break;

                default:
                    Console.WriteLine("Opción inválida");
                    Console.ReadKey();
                    break;
            }

        } while (opcion != 4);
    }

    // ==========================
    // MENÚ LIBROS
    // ==========================

    static void MenuLibros()
    {
        int opcion;

        do
        {
            Console.Clear();
            Console.WriteLine("===== GESTIÓN DE LIBROS =====");
            Console.WriteLine("1. Registrar libro");
            Console.WriteLine("2. Buscar libro");
            Console.WriteLine("3. Listar libros");
            Console.WriteLine("4. Eliminar libro");
            Console.WriteLine("5. Regresar");

            int.TryParse(Console.ReadLine(), out opcion);

            switch (opcion)
            {
                case 1:
                    RegistrarLibro();
                    break;

                case 2:
                    BuscarLibro();
                    break;

                case 3:
                    ListarLibros();
                    break;

                case 4:
                    EliminarLibro();
                    break;
            }

        } while (opcion != 5);
    }

    static void RegistrarLibro()
    {
        try
        {
            if (totalLibros >= 10)
            {
                Console.WriteLine("Límite de libros alcanzado");
                Console.ReadKey();
                return;
            }

            Libro libro = new Libro();

            Console.Write("Código: ");
            libro.Codigo = Console.ReadLine();

            if (libro.Codigo == "")
            {
                Console.WriteLine("Campo vacío");
                Console.ReadKey();
                return;
            }

            Console.Write("Título: ");
            libro.Titulo = Console.ReadLine();

            Console.Write("Autor: ");
            libro.Autor = Console.ReadLine();

            Console.Write("Editorial: ");
            libro.Editorial = Console.ReadLine();

            Console.Write("Año: ");
            while (!int.TryParse(Console.ReadLine(), out libro.Anio))
            {
                Console.WriteLine("Ingrese un número válido");
            }

            Console.WriteLine("Categorías:");
            for (int i = 0; i < categorias.GetLength(0); i++)
            {
                Console.WriteLine(categorias[i, 0] + ". " + categorias[i, 1]);
            }

            Console.Write("Seleccione categoría: ");
            string opcion = Console.ReadLine();

            if (opcion == "1")
                libro.Categoria = categorias[0, 1];
            else if (opcion == "2")
                libro.Categoria = categorias[1, 1];
            else
                libro.Categoria = categorias[2, 1];

            Console.Write("Cantidad disponible: ");

            while (!int.TryParse(Console.ReadLine(), out libro.Cantidad))
            {
                Console.WriteLine("Ingrese número válido");
            }

            libros[totalLibros] = libro;
            totalLibros++;

            GuardarLibros();

            Console.WriteLine("Libro registrado");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.ReadKey();
    }

    static void BuscarLibro()
    {
        Console.Write("Ingrese código: ");
        string codigo = Console.ReadLine();

        bool encontrado = false;

        for (int i = 0; i < totalLibros; i++)
        {
            if (libros[i].Codigo.ToLower().Contains(codigo.ToLower()))
            {
                Console.WriteLine("Título: " + libros[i].Titulo);
                Console.WriteLine("Autor: " + libros[i].Autor);
                Console.WriteLine("Cantidad: " + libros[i].Cantidad);

                encontrado = true;
            }
        }

        if (!encontrado)
        {
            Console.WriteLine("Libro no encontrado");
        }

        Console.ReadKey();
    }

    static void ListarLibros()
    {
        Console.Clear();

        for (int i = 0; i < totalLibros; i++)
        {
            Console.WriteLine("================================");
            Console.WriteLine("Código: " + libros[i].Codigo);
            Console.WriteLine("Título: " + libros[i].Titulo);
            Console.WriteLine("Autor: " + libros[i].Autor);
            Console.WriteLine("Editorial: " + libros[i].Editorial);
            Console.WriteLine("Año: " + libros[i].Anio);
            Console.WriteLine("Categoría: " + libros[i].Categoria);
            Console.WriteLine("Cantidad: " + libros[i].Cantidad);
        }

        Console.ReadKey();
    }

    static void EliminarLibro()
    {
        Console.Write("Código a eliminar: ");
        string codigo = Console.ReadLine();

        for (int i = 0; i < totalLibros; i++)
        {
            if (libros[i].Codigo == codigo)
            {
                for (int j = i; j < totalLibros - 1; j++)
                {
                    libros[j] = libros[j + 1];
                }

                totalLibros--;

                GuardarLibros();

                Console.WriteLine("Libro eliminado");
                Console.ReadKey();
                return;
            }
        }

        Console.WriteLine("Libro no encontrado");
        Console.ReadKey();
    }

    // ==========================
    // USUARIOS
    // ==========================

    static void MenuUsuarios()
    {
        int opcion;

        do
        {
            Console.Clear();

            Console.WriteLine("===== GESTIÓN DE USUARIOS =====");
            Console.WriteLine("1. Registrar usuario");
            Console.WriteLine("2. Buscar por carné");
            Console.WriteLine("3. Buscar por nombre");
            Console.WriteLine("4. Listar usuarios");
            Console.WriteLine("5. Regresar");

            int.TryParse(Console.ReadLine(), out opcion);

            switch (opcion)
            {
                case 1:
                    RegistrarUsuario();
                    break;

                case 2:
                    BuscarUsuarioCarne();
                    break;

                case 3:
                    BuscarUsuarioNombre();
                    break;

                case 4:
                    ListarUsuarios();
                    break;
            }

        } while (opcion != 5);
    }

    static void RegistrarUsuario()
    {
        try
        {
            if (totalUsuarios >= 5)
            {
                Console.WriteLine("Límite alcanzado");
                Console.ReadKey();
                return;
            }

            Usuario u = new Usuario();

            Console.Write("Carné: ");
            u.Carne = Console.ReadLine();

            Console.Write("Nombre completo: ");
            u.Nombre = Console.ReadLine();

            Console.Write("Carrera: ");
            u.Carrera = Console.ReadLine();

            Console.Write("Correo: ");
            u.Correo = Console.ReadLine();

            if (!u.Correo.Contains("@"))
            {
                Console.WriteLine("Correo inválido");
                Console.ReadKey();
                return;
            }

            Console.Write("Teléfono: ");
            u.Telefono = Console.ReadLine();

            u.Estado = "Activo";

            usuarios[totalUsuarios] = u;
            totalUsuarios++;

            GuardarUsuarios();

            Console.WriteLine("Usuario registrado");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.ReadKey();
    }

    static void BuscarUsuarioCarne()
    {
        Console.Write("Carné: ");
        string carne = Console.ReadLine();

        for (int i = 0; i < totalUsuarios; i++)
        {
            if (usuarios[i].Carne == carne)
            {
                Console.WriteLine("Nombre: " + usuarios[i].Nombre);
                Console.WriteLine("Carrera: " + usuarios[i].Carrera);
            }
        }

        Console.ReadKey();
    }

    static void BuscarUsuarioNombre()
    {
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();

        for (int i = 0; i < totalUsuarios; i++)
        {
            if (usuarios[i].Nombre.ToLower().Contains(nombre.ToLower()))
            {
                Console.WriteLine(usuarios[i].Nombre);
            }
        }

        Console.ReadKey();
    }

    static void ListarUsuarios()
    {
        for (int i = 0; i < totalUsuarios; i++)
        {
            Console.WriteLine("======================");
            Console.WriteLine("Carné: " + usuarios[i].Carne);
            Console.WriteLine("Nombre: " + usuarios[i].Nombre);
            Console.WriteLine("Carrera: " + usuarios[i].Carrera);
            Console.WriteLine("Correo: " + usuarios[i].Correo);
            Console.WriteLine("Teléfono: " + usuarios[i].Telefono);
            Console.WriteLine("Estado: " + usuarios[i].Estado);
        }

        Console.ReadKey();
    }

    // ==========================
    // PRÉSTAMOS
    // ==========================

    static void MenuPrestamos()
    {
        int opcion;

        do
        {
            Console.Clear();

            Console.WriteLine("===== PRÉSTAMOS =====");
            Console.WriteLine("1. Registrar préstamo");
            Console.WriteLine("2. Registrar devolución");
            Console.WriteLine("3. Consultar préstamos activos");
            Console.WriteLine("4. Regresar");

            int.TryParse(Console.ReadLine(), out opcion);

            switch (opcion)
            {
                case 1:
                    RegistrarPrestamo();
                    break;

                case 2:
                    RegistrarDevolucion();
                    break;

                case 3:
                    ConsultarPrestamos();
                    break;
            }

        } while (opcion != 4);
    }

    static void RegistrarPrestamo()
    {
        if (totalPrestamos >= 10)
        {
            Console.WriteLine("Límite alcanzado");
            Console.ReadKey();
            return;
        }

        Prestamo p = new Prestamo();

        Console.Write("Código libro: ");
        p.CodigoLibro = Console.ReadLine();

        bool disponible = false;

        for (int i = 0; i < totalLibros; i++)
        {
            if (libros[i].Codigo == p.CodigoLibro)
            {
                if (libros[i].Cantidad > 0)
                {
                    libros[i].Cantidad--;
                    disponible = true;
                }
            }
        }

        if (!disponible)
        {
            Console.WriteLine("Libro no disponible");
            Console.ReadKey();
            return;
        }

        Console.Write("Carné usuario: ");
        p.CarneUsuario = Console.ReadLine();

        p.FechaPrestamo = DateTime.Now.ToString();
        p.Estado = "Activo";

        prestamos[totalPrestamos] = p;
        totalPrestamos++;

        GuardarPrestamos();
        GuardarLibros();

        Console.WriteLine("Préstamo registrado");

        Console.ReadKey();
    }

    static void RegistrarDevolucion()
    {
        Console.Write("Código libro: ");
        string codigo = Console.ReadLine();

        for (int i = 0; i < totalPrestamos; i++)
        {
            if (prestamos[i].CodigoLibro == codigo &&
                prestamos[i].Estado == "Activo")
            {
                prestamos[i].Estado = "Devuelto";

                for (int j = 0; j < totalLibros; j++)
                {
                    if (libros[j].Codigo == codigo)
                    {
                        libros[j].Cantidad++;
                    }
                }

                GuardarPrestamos();
                GuardarLibros();

                Console.WriteLine("Devolución registrada");
                Console.ReadKey();
                return;
            }
        }

        Console.WriteLine("Préstamo no encontrado");
        Console.ReadKey();
    }

    static void ConsultarPrestamos()
    {
        for (int i = 0; i < totalPrestamos; i++)
        {
            if (prestamos[i].Estado == "Activo")
            {
                Console.WriteLine("=====================");
                Console.WriteLine("Libro: " + prestamos[i].CodigoLibro);
                Console.WriteLine("Usuario: " + prestamos[i].CarneUsuario);
                Console.WriteLine("Fecha: " + prestamos[i].FechaPrestamo);
            }
        }

        Console.ReadKey();
    }

    // ==========================
    // ARCHIVOS
    // ==========================

    static void GuardarLibros()
    {
        StreamWriter sw = new StreamWriter("libros.csv");

        for (int i = 0; i < totalLibros; i++)
        {
            sw.WriteLine(
                libros[i].Codigo + "," +
                libros[i].Titulo + "," +
                libros[i].Autor + "," +
                libros[i].Editorial + "," +
                libros[i].Anio + "," +
                libros[i].Categoria + "," +
                libros[i].Cantidad
            );
        }

        sw.Close();
    }

    static void CargarLibros()
    {
        if (File.Exists("libros.csv"))
        {
            string[] lineas = File.ReadAllLines("libros.csv");

            foreach (string linea in lineas)
            {
                string[] datos = linea.Split(',');

                Libro l = new Libro();

                l.Codigo = datos[0];
                l.Titulo = datos[1];
                l.Autor = datos[2];
                l.Editorial = datos[3];
                l.Anio = int.Parse(datos[4]);
                l.Categoria = datos[5];
                l.Cantidad = int.Parse(datos[6]);

                libros[totalLibros] = l;
                totalLibros++;
            }
        }
    }

    static void GuardarUsuarios()
    {
        StreamWriter sw = new StreamWriter("usuarios.txt");

        for (int i = 0; i < totalUsuarios; i++)
        {
            sw.WriteLine(
                usuarios[i].Carne + "|" +
                usuarios[i].Nombre + "|" +
                usuarios[i].Carrera + "|" +
                usuarios[i].Correo + "|" +
                usuarios[i].Telefono + "|" +
                usuarios[i].Estado
            );
        }

        sw.Close();
    }

    static void CargarUsuarios()
    {
        if (File.Exists("usuarios.txt"))
        {
            string[] lineas = File.ReadAllLines("usuarios.txt");

            foreach (string linea in lineas)
            {
                string[] datos = linea.Split('|');

                Usuario u = new Usuario();

                u.Carne = datos[0];
                u.Nombre = datos[1];
                u.Carrera = datos[2];
                u.Correo = datos[3];
                u.Telefono = datos[4];
                u.Estado = datos[5];

                usuarios[totalUsuarios] = u;
                totalUsuarios++;
            }
        }
    }

    static void GuardarPrestamos()
    {
        StreamWriter sw = new StreamWriter("prestamos.txt");

        for (int i = 0; i < totalPrestamos; i++)
        {
            sw.WriteLine(
                prestamos[i].CodigoLibro + "|" +
                prestamos[i].CarneUsuario + "|" +
                prestamos[i].FechaPrestamo + "|" +
                prestamos[i].Estado
            );
        }

        sw.Close();
    }

    static void CargarPrestamos()
    {
        if (File.Exists("prestamos.txt"))
        {
            string[] lineas = File.ReadAllLines("prestamos.txt");

            foreach (string linea in lineas)
            {
                string[] datos = linea.Split('|');

                Prestamo p = new Prestamo();

                p.CodigoLibro = datos[0];
                p.CarneUsuario = datos[1];
                p.FechaPrestamo = datos[2];
                p.Estado = datos[3];

                prestamos[totalPrestamos] = p;
                totalPrestamos++;
            }
        }
    }
}
