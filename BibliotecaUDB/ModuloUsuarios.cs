// ModuloUsuarios.cs
// Modulo B - Gestion de Usuarios
// Para registrar, buscar y listar usuarios de la biblioteca

using System;

namespace BibliotecaUDB
{
    static class ModuloUsuarios
    {
        public static void MostrarMenu(Usuario[] usuarios, ref int totalUsuarios)
        {
            int opcion = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("================================================");
                Console.WriteLine("        MODULO B - GESTION DE USUARIOS         ");
                Console.WriteLine("================================================");
                Console.WriteLine("  1. Registrar nuevo usuario");
                Console.WriteLine("  2. Buscar usuario por carne");
                Console.WriteLine("  3. Buscar usuario por nombre");
                Console.WriteLine("  4. Listar todos los usuarios");
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
                        RegistrarUsuario(usuarios, ref totalUsuarios);
                        break;
                    case 2:
                        BuscarPorCarne(usuarios, totalUsuarios);
                        break;
                    case 3:
                        BuscarPorNombre(usuarios, totalUsuarios);
                        break;
                    case 4:
                        ListarUsuarios(usuarios, totalUsuarios);
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

        static void RegistrarUsuario(Usuario[] usuarios, ref int totalUsuarios)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("          REGISTRAR NUEVO USUARIO              ");
            Console.WriteLine("================================================");

            if (totalUsuarios >= usuarios.Length)
            {
                Console.WriteLine("\n  ERROR: Se alcanzo el limite de usuarios (" + usuarios.Length + ").");
                Console.ReadKey();
                return;
            }

            Usuario nuevo = new Usuario();

            // carne
            bool carneValido = false;
            while (!carneValido)
            {
                Console.Write("\n  Carne (8 digitos numericos): ");
                nuevo.Carne = Console.ReadLine().Trim();

                if (!Validaciones.ValidarCarne(nuevo.Carne))
                {
                    Console.WriteLine("  Error: el carne debe tener exactamente 8 digitos numericos.");
                }
                else if (BuscarIndicePorCarne(usuarios, totalUsuarios, nuevo.Carne) >= 0)
                {
                    Console.WriteLine("  Error: ya existe un usuario con ese carne.");
                }
                else
                {
                    carneValido = true;
                }
            }

            // nombre completo
            do
            {
                Console.Write("  Nombre completo: ");
                nuevo.NombreCompleto = Console.ReadLine().Trim();
                if (!Validaciones.NoEstaVacio(nuevo.NombreCompleto))
                    Console.WriteLine("  Error: el nombre no puede estar vacio.");
            } while (!Validaciones.NoEstaVacio(nuevo.NombreCompleto));

            // carrera
            do
            {
                Console.Write("  Carrera: ");
                nuevo.Carrera = Console.ReadLine().Trim();
                if (!Validaciones.NoEstaVacio(nuevo.Carrera))
                    Console.WriteLine("  Error: la carrera no puede estar vacia.");
            } while (!Validaciones.NoEstaVacio(nuevo.Carrera));

            // correo electronico
            bool correoValido = false;
            while (!correoValido)
            {
                Console.Write("  Correo electronico: ");
                nuevo.Correo = Console.ReadLine().Trim();

                if (!Validaciones.ValidarCorreo(nuevo.Correo))
                    Console.WriteLine("  Error: ingrese un correo valido (debe tener @ y un punto despues).");
                else
                    correoValido = true;
            }

            // telefono
            do
            {
                Console.Write("  Telefono (ej: 7000-0000): ");
                nuevo.Telefono = Console.ReadLine().Trim();
                if (!Validaciones.NoEstaVacio(nuevo.Telefono))
                    Console.WriteLine("  Error: el telefono no puede estar vacio.");
            } while (!Validaciones.NoEstaVacio(nuevo.Telefono));

            // estado por defecto activo
            nuevo.Estado = "activo";

            usuarios[totalUsuarios] = nuevo;
            totalUsuarios++;

            Console.WriteLine("\n  Usuario registrado exitosamente. Estado: activo");
            Console.ReadKey();
        }

        static void BuscarPorCarne(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("          BUSCAR USUARIO POR CARNE             ");
            Console.WriteLine("================================================");

            if (totalUsuarios == 0)
            {
                Console.WriteLine("\n  No hay usuarios registrados.");
                Console.ReadKey();
                return;
            }

            Console.Write("\n  Ingrese el carne del usuario: ");
            string carne = Console.ReadLine().Trim();

            int indice = BuscarIndicePorCarne(usuarios, totalUsuarios, carne);

            if (indice < 0)
            {
                Console.WriteLine("  No se encontro el usuario con carne: " + carne);
            }
            else
            {
                MostrarDetallesUsuario(usuarios[indice]);
            }

            Console.ReadKey();
        }

        // busqueda por nombre - puede haber varios resultados con el mismo nombre
        static void BuscarPorNombre(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("          BUSCAR USUARIO POR NOMBRE            ");
            Console.WriteLine("================================================");

            if (totalUsuarios == 0)
            {
                Console.WriteLine("\n  No hay usuarios registrados.");
                Console.ReadKey();
                return;
            }

            Console.Write("\n  Ingrese el nombre a buscar: ");
            string nombre = Console.ReadLine().Trim().ToLower();

            int encontrados = 0;

            // recorrer todos los usuarios y buscar coincidencias parciales
            for (int i = 0; i < totalUsuarios; i++)
            {
                if (usuarios[i].NombreCompleto.ToLower().Contains(nombre))
                {
                    MostrarDetallesUsuario(usuarios[i]);
                    encontrados++;
                }
            }

            if (encontrados == 0)
                Console.WriteLine("  No se encontraron usuarios con ese nombre.");
            else
                Console.WriteLine("\n  Total encontrados: " + encontrados);

            Console.ReadKey();
        }

        public static void ListarUsuarios(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("           LISTADO DE USUARIOS                 ");
            Console.WriteLine("================================================");

            if (totalUsuarios == 0)
            {
                Console.WriteLine("\n  No hay usuarios registrados.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n  Total: " + totalUsuarios + "/" + usuarios.Length);
            Console.WriteLine();
            Console.WriteLine("  {0,-10} {1,-25} {2,-20} {3,-10}",
                "Carne", "Nombre", "Carrera", "Estado");
            Console.WriteLine("  " + new string('-', 70));

            for (int i = 0; i < totalUsuarios; i++)
            {
                string nombre = usuarios[i].NombreCompleto;
                if (nombre.Length > 23) nombre = nombre.Substring(0, 20) + "...";

                string carrera = usuarios[i].Carrera;
                if (carrera.Length > 18) carrera = carrera.Substring(0, 15) + "...";

                Console.WriteLine("  {0,-10} {1,-25} {2,-20} {3,-10}",
                    usuarios[i].Carne,
                    nombre,
                    carrera,
                    usuarios[i].Estado);
            }

            Console.WriteLine();
            Console.ReadKey();
        }

        static void MostrarDetallesUsuario(Usuario usuario)
        {
            Console.WriteLine("\n  ----- Datos del usuario -----");
            Console.WriteLine("  Carne:    " + usuario.Carne);
            Console.WriteLine("  Nombre:   " + usuario.NombreCompleto);
            Console.WriteLine("  Carrera:  " + usuario.Carrera);
            Console.WriteLine("  Correo:   " + usuario.Correo);
            Console.WriteLine("  Telefono: " + usuario.Telefono);
            Console.WriteLine("  Estado:   " + usuario.Estado);
        }

        // metodo publico para que el modulo de prestamos pueda buscar usuarios
        public static int BuscarIndicePorCarne(Usuario[] usuarios, int total, string carne)
        {
            for (int i = 0; i < total; i++)
            {
                if (usuarios[i].Carne == carne)
                    return i;
            }
            return -1;
        }
    }
}
