// Sistema de Gestion de Biblioteca Universitaria
// Universidad Don Bosco - Programacion de Algoritmos
// Desafio Final Ciclo I - 2026
//
// Estudiante: Jonathan Daniel Grande Ramirez
// Carnet: GR231632

using System;
using System.IO;

namespace BibliotecaUDB
{
    // structs para modelar las entidades del sistema
    struct Libro
    {
        public string Codigo;
        public string Titulo;
        public string Autor;
        public string Editorial;
        public int AnioPublicacion;
        public string Categoria;
        public int EjemplaresDisponibles;
    }

    struct Usuario
    {
        public string Carne;
        public string NombreCompleto;
        public string Carrera;
        public string Correo;
        public string Telefono;
        public string Estado;
    }

    struct Prestamo
    {
        public string IdPrestamo;
        public string CarneUsuario;
        public string CodigoLibro;
        public string FechaPrestamo;
        public string FechaDevolucion;
        public string Estado;
    }

    class Program
    {
        static Libro[]    libros    = new Libro[10];
        static Usuario[]  usuarios  = new Usuario[5];
        static Prestamo[] prestamos = new Prestamo[10];

        static int totalLibros = 0, totalUsuarios = 0, totalPrestamos = 0;

        // matriz para el reporte: fila=prestamo, col 0=carne, 1=codigoLibro, 2=estado
        static string[,] reporteMatriz = new string[10, 3];

        static string rutaLibros    = "Data/libros.csv";
        static string rutaUsuarios  = "Data/usuarios.txt";
        static string rutaPrestamos = "Data/prestamos.txt";
        static string rutaReporte   = "Data/reporte.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Cargando datos...");
            CargarDatos();
            SincronizarMatriz();

            int opcion = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("SISTEMA DE BIBLIOTECA - UDB");
                Console.WriteLine("----------------------------");
                Console.WriteLine("1. Gestion de Libros");
                Console.WriteLine("2. Gestion de Usuarios");
                Console.WriteLine("3. Gestion de Prestamos");
                Console.WriteLine("4. Generar Reporte");
                Console.WriteLine("5. Guardar datos");
                Console.WriteLine("6. Salir");
                Console.Write("Opcion: ");

                try { opcion = int.Parse(Console.ReadLine()); }
                catch { opcion = -1; }

                switch (opcion)
                {
                    case 1: MenuLibros();    break;
                    case 2: MenuUsuarios();  break;
                    case 3: MenuPrestamos(); break;
                    case 4: GenerarReporte(); break;
                    case 5:
                        GuardarDatos();
                        Console.WriteLine("Datos guardados.");
                        Console.ReadKey();
                        break;
                    case 6: Salir(); break;
                    default:
                        Console.WriteLine("Opcion no valida.");
                        Console.ReadKey();
                        break;
                }
            } while (opcion != 6);
        }

        // =============================================
        // MODULO A - LIBROS
        // =============================================

        static void MenuLibros()
        {
            int opcion = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("LIBROS");
                Console.WriteLine("------");
                Console.WriteLine("1. Registrar libro");
                Console.WriteLine("2. Buscar por codigo");
                Console.WriteLine("3. Listar todos");
                Console.WriteLine("4. Eliminar libro");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                try { opcion = int.Parse(Console.ReadLine()); }
                catch { opcion = -1; }

                switch (opcion)
                {
                    case 1: RegistrarLibro(); break;
                    case 2: BuscarLibro();    break;
                    case 3: ListarLibros();   break;
                    case 4: EliminarLibro();  break;
                    case 0: break;
                    default: Console.WriteLine("Opcion no valida."); Console.ReadKey(); break;
                }
            } while (opcion != 0);
        }

        static void RegistrarLibro()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR LIBRO\n");

            if (totalLibros >= libros.Length)
            {
                Console.WriteLine("Se alcanzo el limite de libros (" + libros.Length + ").");
                Console.ReadKey();
                return;
            }

            Libro nuevo = new Libro();

            // validar codigo
            bool ok = false;
            while (!ok)
            {
                Console.Write("Codigo (8 chars alfanumericos, ej: LIB00001): ");
                nuevo.Codigo = Console.ReadLine().Trim().ToUpper();
                if (!ValidarCodigoLibro(nuevo.Codigo))
                    Console.WriteLine("Error: debe tener exactamente 8 caracteres alfanumericos.");
                else if (BuscarIndicePorCodigo(nuevo.Codigo) >= 0)
                    Console.WriteLine("Error: ya existe un libro con ese codigo.");
                else
                    ok = true;
            }

            do {
                Console.Write("Titulo: ");
                nuevo.Titulo = Console.ReadLine().Trim();
                if (!NoVacio(nuevo.Titulo)) Console.WriteLine("Campo obligatorio.");
            } while (!NoVacio(nuevo.Titulo));

            do {
                Console.Write("Autor: ");
                nuevo.Autor = Console.ReadLine().Trim();
                if (!NoVacio(nuevo.Autor)) Console.WriteLine("Campo obligatorio.");
            } while (!NoVacio(nuevo.Autor));

            do {
                Console.Write("Editorial: ");
                nuevo.Editorial = Console.ReadLine().Trim();
                if (!NoVacio(nuevo.Editorial)) Console.WriteLine("Campo obligatorio.");
            } while (!NoVacio(nuevo.Editorial));

            ok = false;
            while (!ok)
            {
                Console.Write("Año de publicacion (1900-" + DateTime.Now.Year + "): ");
                try
                {
                    nuevo.AnioPublicacion = int.Parse(Console.ReadLine());
                    if (nuevo.AnioPublicacion < 1900 || nuevo.AnioPublicacion > DateTime.Now.Year)
                        Console.WriteLine("Año fuera de rango.");
                    else
                        ok = true;
                }
                catch { Console.WriteLine("Ingrese solo numeros."); }
            }

            do {
                Console.Write("Categoria: ");
                nuevo.Categoria = Console.ReadLine().Trim();
                if (!NoVacio(nuevo.Categoria)) Console.WriteLine("Campo obligatorio.");
            } while (!NoVacio(nuevo.Categoria));

            ok = false;
            while (!ok)
            {
                Console.Write("Ejemplares disponibles: ");
                try
                {
                    nuevo.EjemplaresDisponibles = int.Parse(Console.ReadLine());
                    if (nuevo.EjemplaresDisponibles < 0) Console.WriteLine("No puede ser negativo.");
                    else ok = true;
                }
                catch { Console.WriteLine("Ingrese un numero entero."); }
            }

            libros[totalLibros++] = nuevo;
            Console.WriteLine("\nLibro registrado correctamente.");
            Console.ReadKey();
        }

        static void BuscarLibro()
        {
            Console.Clear();
            Console.Write("Codigo del libro: ");
            int i = BuscarIndicePorCodigo(Console.ReadLine().Trim().ToUpper());
            if (i < 0) Console.WriteLine("No se encontro ningun libro con ese codigo.");
            else MostrarLibro(libros[i]);
            Console.ReadKey();
        }

        static void ListarLibros()
        {
            Console.Clear();
            if (totalLibros == 0) { Console.WriteLine("No hay libros registrados."); Console.ReadKey(); return; }

            Console.WriteLine("Total: " + totalLibros + "/" + libros.Length + "\n");
            for (int i = 0; i < totalLibros; i++)
                Console.WriteLine(libros[i].Codigo + " | " + libros[i].Titulo + " | " + libros[i].Autor + " | " + libros[i].EjemplaresDisponibles + " ejmp.");
            Console.ReadKey();
        }

        static void EliminarLibro()
        {
            Console.Clear();
            Console.Write("Codigo del libro a eliminar: ");
            int i = BuscarIndicePorCodigo(Console.ReadLine().Trim().ToUpper());

            if (i < 0) { Console.WriteLine("Libro no encontrado."); Console.ReadKey(); return; }

            MostrarLibro(libros[i]);
            Console.Write("\n¿Confirmar eliminacion? (s/n): ");
            if (Console.ReadLine().Trim().ToLower() != "s") { Console.WriteLine("Cancelado."); Console.ReadKey(); return; }

            // desplazar elementos para llenar el espacio
            for (int j = i; j < totalLibros - 1; j++)
                libros[j] = libros[j + 1];

            libros[totalLibros - 1] = new Libro();
            totalLibros--;
            Console.WriteLine("Libro eliminado.");
            Console.ReadKey();
        }

        static void MostrarLibro(Libro l)
        {
            Console.WriteLine("\nCodigo:     " + l.Codigo);
            Console.WriteLine("Titulo:     " + l.Titulo);
            Console.WriteLine("Autor:      " + l.Autor);
            Console.WriteLine("Editorial:  " + l.Editorial);
            Console.WriteLine("Año:        " + l.AnioPublicacion);
            Console.WriteLine("Categoria:  " + l.Categoria);
            Console.WriteLine("Ejemplares: " + l.EjemplaresDisponibles);
        }

        static int BuscarIndicePorCodigo(string codigo)
        {
            for (int i = 0; i < totalLibros; i++)
                if (libros[i].Codigo == codigo) return i;
            return -1;
        }

        // =============================================
        // MODULO B - USUARIOS
        // =============================================

        static void MenuUsuarios()
        {
            int opcion = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("USUARIOS");
                Console.WriteLine("--------");
                Console.WriteLine("1. Registrar usuario");
                Console.WriteLine("2. Buscar por carne");
                Console.WriteLine("3. Buscar por nombre");
                Console.WriteLine("4. Listar todos");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                try { opcion = int.Parse(Console.ReadLine()); }
                catch { opcion = -1; }

                switch (opcion)
                {
                    case 1: RegistrarUsuario();    break;
                    case 2: BuscarPorCarne();      break;
                    case 3: BuscarPorNombre();     break;
                    case 4: ListarUsuarios();      break;
                    case 0: break;
                    default: Console.WriteLine("Opcion no valida."); Console.ReadKey(); break;
                }
            } while (opcion != 0);
        }

        static void RegistrarUsuario()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR USUARIO\n");

            if (totalUsuarios >= usuarios.Length)
            {
                Console.WriteLine("Se alcanzo el limite de usuarios (" + usuarios.Length + ").");
                Console.ReadKey();
                return;
            }

            Usuario nuevo = new Usuario();
            bool ok = false;

            while (!ok)
            {
                Console.Write("Carne (8 digitos): ");
                nuevo.Carne = Console.ReadLine().Trim();
                if (!ValidarCarne(nuevo.Carne))
                    Console.WriteLine("Error: debe tener exactamente 8 digitos numericos.");
                else if (BuscarIndicePorCarne(nuevo.Carne) >= 0)
                    Console.WriteLine("Error: ya existe un usuario con ese carne.");
                else
                    ok = true;
            }

            do {
                Console.Write("Nombre completo: ");
                nuevo.NombreCompleto = Console.ReadLine().Trim();
                if (!NoVacio(nuevo.NombreCompleto)) Console.WriteLine("Campo obligatorio.");
            } while (!NoVacio(nuevo.NombreCompleto));

            do {
                Console.Write("Carrera: ");
                nuevo.Carrera = Console.ReadLine().Trim();
                if (!NoVacio(nuevo.Carrera)) Console.WriteLine("Campo obligatorio.");
            } while (!NoVacio(nuevo.Carrera));

            ok = false;
            while (!ok)
            {
                Console.Write("Correo electronico: ");
                nuevo.Correo = Console.ReadLine().Trim();
                if (!ValidarCorreo(nuevo.Correo)) Console.WriteLine("Correo invalido (necesita @ y un punto despues).");
                else ok = true;
            }

            do {
                Console.Write("Telefono: ");
                nuevo.Telefono = Console.ReadLine().Trim();
                if (!NoVacio(nuevo.Telefono)) Console.WriteLine("Campo obligatorio.");
            } while (!NoVacio(nuevo.Telefono));

            nuevo.Estado = "activo";
            usuarios[totalUsuarios++] = nuevo;
            Console.WriteLine("\nUsuario registrado. Estado: activo");
            Console.ReadKey();
        }

        static void BuscarPorCarne()
        {
            Console.Clear();
            Console.Write("Carne: ");
            int i = BuscarIndicePorCarne(Console.ReadLine().Trim());
            if (i < 0) Console.WriteLine("Usuario no encontrado.");
            else MostrarUsuario(usuarios[i]);
            Console.ReadKey();
        }

        static void BuscarPorNombre()
        {
            Console.Clear();
            Console.Write("Nombre a buscar: ");
            string nombre = Console.ReadLine().Trim().ToLower();
            int encontrados = 0;

            for (int i = 0; i < totalUsuarios; i++)
                if (usuarios[i].NombreCompleto.ToLower().Contains(nombre))
                {
                    MostrarUsuario(usuarios[i]);
                    encontrados++;
                }

            if (encontrados == 0) Console.WriteLine("No se encontraron resultados.");
            else Console.WriteLine("\nEncontrados: " + encontrados);
            Console.ReadKey();
        }

        static void ListarUsuarios()
        {
            Console.Clear();
            if (totalUsuarios == 0) { Console.WriteLine("No hay usuarios registrados."); Console.ReadKey(); return; }

            Console.WriteLine("Total: " + totalUsuarios + "/" + usuarios.Length + "\n");
            for (int i = 0; i < totalUsuarios; i++)
                Console.WriteLine(usuarios[i].Carne + " | " + usuarios[i].NombreCompleto + " | " + usuarios[i].Carrera + " | " + usuarios[i].Estado);
            Console.ReadKey();
        }

        static void MostrarUsuario(Usuario u)
        {
            Console.WriteLine("\nCarne:    " + u.Carne);
            Console.WriteLine("Nombre:   " + u.NombreCompleto);
            Console.WriteLine("Carrera:  " + u.Carrera);
            Console.WriteLine("Correo:   " + u.Correo);
            Console.WriteLine("Telefono: " + u.Telefono);
            Console.WriteLine("Estado:   " + u.Estado);
        }

        static int BuscarIndicePorCarne(string carne)
        {
            for (int i = 0; i < totalUsuarios; i++)
                if (usuarios[i].Carne == carne) return i;
            return -1;
        }

        // =============================================
        // MODULO C - PRESTAMOS
        // =============================================

        static void MenuPrestamos()
        {
            int opcion = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("PRESTAMOS");
                Console.WriteLine("---------");
                Console.WriteLine("1. Registrar prestamo");
                Console.WriteLine("2. Registrar devolucion");
                Console.WriteLine("3. Historial de usuario");
                Console.WriteLine("4. Listar activos");
                Console.WriteLine("5. Actualizar estado");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                try { opcion = int.Parse(Console.ReadLine()); }
                catch { opcion = -1; }

                switch (opcion)
                {
                    case 1: RegistrarPrestamo();   break;
                    case 2: RegistrarDevolucion(); break;
                    case 3: HistorialUsuario();    break;
                    case 4: ListarActivos();       break;
                    case 5: ActualizarEstado();    break;
                    case 0: break;
                    default: Console.WriteLine("Opcion no valida."); Console.ReadKey(); break;
                }
            } while (opcion != 0);
        }

        static void RegistrarPrestamo()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR PRESTAMO\n");

            if (totalPrestamos >= prestamos.Length)
            {
                Console.WriteLine("Se alcanzo el limite de prestamos.");
                Console.ReadKey();
                return;
            }

            Prestamo nuevo = new Prestamo();

            // buscar usuario activo
            int iu = -1;
            while (iu < 0)
            {
                Console.Write("Carne del usuario: ");
                iu = BuscarIndicePorCarne(Console.ReadLine().Trim());
                if (iu < 0)
                    Console.WriteLine("Usuario no encontrado.");
                else if (usuarios[iu].Estado == "inactivo")
                {
                    Console.WriteLine("El usuario esta inactivo, no puede hacer prestamos.");
                    iu = -1;
                }
                else
                {
                    nuevo.CarneUsuario = usuarios[iu].Carne;
                    Console.WriteLine("Usuario: " + usuarios[iu].NombreCompleto);
                }
            }

            // buscar libro con ejemplares disponibles
            int il = -1;
            while (il < 0)
            {
                Console.Write("Codigo del libro: ");
                il = BuscarIndicePorCodigo(Console.ReadLine().Trim().ToUpper());
                if (il < 0)
                    Console.WriteLine("Libro no encontrado.");
                else if (libros[il].EjemplaresDisponibles <= 0)
                {
                    Console.WriteLine("No hay ejemplares disponibles.");
                    il = -1;
                }
                else
                {
                    nuevo.CodigoLibro = libros[il].Codigo;
                    Console.WriteLine("Libro: " + libros[il].Titulo);
                }
            }

            bool ok = false;
            while (!ok)
            {
                Console.Write("Fecha de prestamo (dd/mm/yyyy): ");
                nuevo.FechaPrestamo = Console.ReadLine().Trim();
                if (!ValidarFecha(nuevo.FechaPrestamo)) Console.WriteLine("Formato incorrecto.");
                else ok = true;
            }

            ok = false;
            while (!ok)
            {
                Console.Write("Fecha de devolucion (dd/mm/yyyy): ");
                nuevo.FechaDevolucion = Console.ReadLine().Trim();
                if (!ValidarFecha(nuevo.FechaDevolucion)) Console.WriteLine("Formato incorrecto.");
                else ok = true;
            }

            nuevo.IdPrestamo = "PRE" + (totalPrestamos + 1).ToString("D5");
            nuevo.Estado = "activo";
            libros[il].EjemplaresDisponibles--;

            // guardar en arreglo y actualizar la matriz
            prestamos[totalPrestamos] = nuevo;
            reporteMatriz[totalPrestamos, 0] = nuevo.CarneUsuario;
            reporteMatriz[totalPrestamos, 1] = nuevo.CodigoLibro;
            reporteMatriz[totalPrestamos, 2] = nuevo.Estado;
            totalPrestamos++;

            Console.WriteLine("\nPrestamo registrado. ID: " + nuevo.IdPrestamo);
            Console.ReadKey();
        }

        static void RegistrarDevolucion()
        {
            Console.Clear();
            Console.Write("ID del prestamo: ");
            int i = BuscarIndicePorId(Console.ReadLine().Trim().ToUpper());

            if (i < 0) { Console.WriteLine("Prestamo no encontrado."); Console.ReadKey(); return; }
            if (prestamos[i].Estado == "devuelto") { Console.WriteLine("Este prestamo ya fue devuelto."); Console.ReadKey(); return; }

            Console.WriteLine("Libro: " + prestamos[i].CodigoLibro + " | Fecha limite: " + prestamos[i].FechaDevolucion);
            Console.Write("Confirmar devolucion (s/n): ");
            if (Console.ReadLine().Trim().ToLower() != "s") { Console.WriteLine("Cancelado."); Console.ReadKey(); return; }

            prestamos[i].Estado = "devuelto";
            reporteMatriz[i, 2] = "devuelto";

            int il = BuscarIndicePorCodigo(prestamos[i].CodigoLibro);
            if (il >= 0) libros[il].EjemplaresDisponibles++;

            Console.WriteLine("Devolucion registrada.");
            Console.ReadKey();
        }

        static void HistorialUsuario()
        {
            Console.Clear();
            Console.Write("Carne del usuario: ");
            string carne = Console.ReadLine().Trim();
            int encontrados = 0;

            for (int i = 0; i < totalPrestamos; i++)
                if (prestamos[i].CarneUsuario == carne)
                {
                    Console.WriteLine(prestamos[i].IdPrestamo + " | " + prestamos[i].CodigoLibro +
                                      " | " + prestamos[i].FechaPrestamo + " | " + prestamos[i].Estado);
                    encontrados++;
                }

            if (encontrados == 0) Console.WriteLine("No hay prestamos para ese carne.");
            Console.ReadKey();
        }

        static void ListarActivos()
        {
            Console.Clear();
            int activos = 0;
            for (int i = 0; i < totalPrestamos; i++)
                if (prestamos[i].Estado == "activo")
                {
                    Console.WriteLine(prestamos[i].IdPrestamo + " | " + prestamos[i].CarneUsuario +
                                      " | " + prestamos[i].CodigoLibro + " | " + prestamos[i].FechaDevolucion);
                    activos++;
                }
            if (activos == 0) Console.WriteLine("No hay prestamos activos.");
            Console.ReadKey();
        }

        static void ActualizarEstado()
        {
            Console.Clear();
            Console.Write("ID del prestamo: ");
            int i = BuscarIndicePorId(Console.ReadLine().Trim().ToUpper());

            if (i < 0) { Console.WriteLine("Prestamo no encontrado."); Console.ReadKey(); return; }

            Console.WriteLine("Estado actual: " + prestamos[i].Estado);
            Console.Write("Nuevo estado (activo/devuelto/vencido): ");
            string nuevo = Console.ReadLine().Trim().ToLower();

            if (nuevo == "activo" || nuevo == "devuelto" || nuevo == "vencido")
            {
                prestamos[i].Estado = nuevo;
                reporteMatriz[i, 2] = nuevo;
                Console.WriteLine("Estado actualizado.");
            }
            else Console.WriteLine("Estado no valido.");
            Console.ReadKey();
        }

        static int BuscarIndicePorId(string id)
        {
            for (int i = 0; i < totalPrestamos; i++)
                if (prestamos[i].IdPrestamo == id) return i;
            return -1;
        }

        // =============================================
        // REPORTE CON MATRIZ
        // =============================================

        static void GenerarReporte()
        {
            Console.Clear();
            if (totalPrestamos == 0) { Console.WriteLine("No hay prestamos registrados."); Console.ReadKey(); return; }

            SincronizarMatriz();

            // contar estados recorriendo la columna 2 de la matriz
            int activos = 0, devueltos = 0, vencidos = 0;
            for (int i = 0; i < totalPrestamos; i++)
            {
                if (reporteMatriz[i, 2] == "activo")   activos++;
                if (reporteMatriz[i, 2] == "devuelto") devueltos++;
                if (reporteMatriz[i, 2] == "vencido")  vencidos++;
            }

            // mostrar la matriz en consola recorriendo filas y columnas
            Console.WriteLine("No. | Carne      | Codigo Libro | Estado");
            Console.WriteLine(new string('-', 45));
            for (int fila = 0; fila < totalPrestamos; fila++)
            {
                Console.Write((fila + 1) + "   ");
                for (int col = 0; col < 3; col++)
                    Console.Write("| " + reporteMatriz[fila, col] + " ");
                Console.WriteLine();
            }

            Console.WriteLine("\nActivos: " + activos + " | Devueltos: " + devueltos + " | Vencidos: " + vencidos);

            // exportar a archivo .txt
            try
            {
                StreamWriter sw = new StreamWriter(rutaReporte, false);
                sw.WriteLine("REPORTE DE PRESTAMOS - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                sw.WriteLine(new string('-', 45));
                sw.WriteLine("No. | Carne      | Codigo Libro | Estado");
                for (int fila = 0; fila < totalPrestamos; fila++)
                {
                    sw.Write((fila + 1) + "   ");
                    for (int col = 0; col < 3; col++)
                        sw.Write("| " + reporteMatriz[fila, col] + " ");
                    sw.WriteLine();
                }
                sw.WriteLine("\nActivos: " + activos);
                sw.WriteLine("Devueltos: " + devueltos);
                sw.WriteLine("Vencidos: " + vencidos);
                sw.Close();
                Console.WriteLine("\nReporte guardado en " + rutaReporte);
            }
            catch (Exception e) { Console.WriteLine("Error al guardar reporte: " + e.Message); }

            Console.ReadKey();
        }

        // reconstruir la matriz desde el arreglo al cargar datos del archivo
        static void SincronizarMatriz()
        {
            for (int i = 0; i < totalPrestamos; i++)
            {
                reporteMatriz[i, 0] = prestamos[i].CarneUsuario;
                reporteMatriz[i, 1] = prestamos[i].CodigoLibro;
                reporteMatriz[i, 2] = prestamos[i].Estado;
            }
        }

        // =============================================
        // ARCHIVOS
        // =============================================

        static void CargarDatos()
        {
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");

            // libros desde CSV
            if (File.Exists(rutaLibros))
            {
                StreamReader sr = new StreamReader(rutaLibros);
                string linea;
                while ((linea = sr.ReadLine()) != null && totalLibros < libros.Length)
                {
                    if (string.IsNullOrEmpty(linea)) continue;
                    string[] p = linea.Split(',');
                    if (p.Length < 7) continue;
                    libros[totalLibros].Codigo    = p[0];
                    libros[totalLibros].Titulo    = p[1];
                    libros[totalLibros].Autor     = p[2];
                    libros[totalLibros].Editorial = p[3];
                    try { libros[totalLibros].AnioPublicacion = int.Parse(p[4]); } catch { libros[totalLibros].AnioPublicacion = 2000; }
                    libros[totalLibros].Categoria = p[5];
                    try { libros[totalLibros].EjemplaresDisponibles = int.Parse(p[6]); } catch { libros[totalLibros].EjemplaresDisponibles = 0; }
                    totalLibros++;
                }
                sr.Close();
            }

            // usuarios desde TXT
            if (File.Exists(rutaUsuarios))
            {
                StreamReader sr = new StreamReader(rutaUsuarios);
                string linea;
                while ((linea = sr.ReadLine()) != null && totalUsuarios < usuarios.Length)
                {
                    if (string.IsNullOrEmpty(linea)) continue;
                    string[] p = linea.Split('|');
                    if (p.Length < 6) continue;
                    usuarios[totalUsuarios].Carne          = p[0];
                    usuarios[totalUsuarios].NombreCompleto = p[1];
                    usuarios[totalUsuarios].Carrera        = p[2];
                    usuarios[totalUsuarios].Correo         = p[3];
                    usuarios[totalUsuarios].Telefono       = p[4];
                    usuarios[totalUsuarios].Estado         = p[5];
                    totalUsuarios++;
                }
                sr.Close();
            }

            // prestamos desde TXT
            if (File.Exists(rutaPrestamos))
            {
                StreamReader sr = new StreamReader(rutaPrestamos);
                string linea;
                while ((linea = sr.ReadLine()) != null && totalPrestamos < prestamos.Length)
                {
                    if (string.IsNullOrEmpty(linea)) continue;
                    string[] p = linea.Split('|');
                    if (p.Length < 6) continue;
                    prestamos[totalPrestamos].IdPrestamo      = p[0];
                    prestamos[totalPrestamos].CarneUsuario    = p[1];
                    prestamos[totalPrestamos].CodigoLibro     = p[2];
                    prestamos[totalPrestamos].FechaPrestamo   = p[3];
                    prestamos[totalPrestamos].FechaDevolucion = p[4];
                    prestamos[totalPrestamos].Estado          = p[5];
                    totalPrestamos++;
                }
                sr.Close();
            }

            Console.WriteLine("Libros: " + totalLibros + " | Usuarios: " + totalUsuarios + " | Prestamos: " + totalPrestamos);
            System.Threading.Thread.Sleep(900);
        }

        static void GuardarDatos()
        {
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");

            StreamWriter sw = new StreamWriter(rutaLibros, false);
            for (int i = 0; i < totalLibros; i++)
                sw.WriteLine(libros[i].Codigo + "," + libros[i].Titulo + "," + libros[i].Autor + "," +
                             libros[i].Editorial + "," + libros[i].AnioPublicacion + "," +
                             libros[i].Categoria + "," + libros[i].EjemplaresDisponibles);
            sw.Close();

            sw = new StreamWriter(rutaUsuarios, false);
            for (int i = 0; i < totalUsuarios; i++)
                sw.WriteLine(usuarios[i].Carne + "|" + usuarios[i].NombreCompleto + "|" + usuarios[i].Carrera + "|" +
                             usuarios[i].Correo + "|" + usuarios[i].Telefono + "|" + usuarios[i].Estado);
            sw.Close();

            sw = new StreamWriter(rutaPrestamos, false);
            for (int i = 0; i < totalPrestamos; i++)
                sw.WriteLine(prestamos[i].IdPrestamo + "|" + prestamos[i].CarneUsuario + "|" + prestamos[i].CodigoLibro + "|" +
                             prestamos[i].FechaPrestamo + "|" + prestamos[i].FechaDevolucion + "|" + prestamos[i].Estado);
            sw.Close();
        }

        static void Salir()
        {
            Console.Write("¿Guardar antes de salir? (s/n): ");
            if (Console.ReadLine().Trim().ToLower() == "s") GuardarDatos();
            Console.WriteLine("Hasta luego.");
            System.Threading.Thread.Sleep(600);
            Environment.Exit(0);
        }

        // =============================================
        // VALIDACIONES
        // =============================================

        static bool ValidarCodigoLibro(string codigo)
        {
            if (codigo == null || codigo.Length != 8) return false;
            foreach (char c in codigo)
                if (!char.IsLetterOrDigit(c)) return false;
            return true;
        }

        static bool ValidarCarne(string carne)
        {
            if (carne == null || carne.Length != 8) return false;
            for (int i = 0; i < carne.Length; i++)
                if (!char.IsDigit(carne[i])) return false;
            return true;
        }

        static bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo)) return false;
            int pos = correo.IndexOf('@');
            if (pos < 0) return false;
            return correo.Substring(pos).IndexOf('.') >= 0;
        }

        static bool ValidarFecha(string fecha)
        {
            if (fecha == null || fecha.Length != 10) return false;
            if (fecha[2] != '/' || fecha[5] != '/') return false;
            try
            {
                int d = int.Parse(fecha.Substring(0, 2));
                int m = int.Parse(fecha.Substring(3, 2));
                int a = int.Parse(fecha.Substring(6, 4));
                if (d < 1 || d > 31 || m < 1 || m > 12 || a < 1900) return false;
            }
            catch { return false; }
            return true;
        }

        static bool NoVacio(string texto)
        {
            return !string.IsNullOrEmpty(texto) && texto.Trim().Length > 0;
        }
    }
}
