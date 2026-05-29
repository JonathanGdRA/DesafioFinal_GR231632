// ManejoArchivos.cs
// Este archivo maneja toda la lectura y escritura de los archivos del sistema
// Los archivos se guardan en la carpeta Data/

using System;
using System.IO;

namespace BibliotecaUDB
{
    static class ManejoArchivos
    {
        // rutas de los archivos
        private static string rutaLibros = "Data/libros.csv";
        private static string rutaUsuarios = "Data/usuarios.txt";
        private static string rutaPrestamos = "Data/prestamos.txt";

        // ============================================================
        // LIBROS - archivo CSV separado por comas
        // ============================================================

        public static void GuardarLibros(Libro[] libros, int totalLibros)
        {
            try
            {
                // crear la carpeta Data si no existe
                if (!Directory.Exists("Data"))
                    Directory.CreateDirectory("Data");

                StreamWriter sw = new StreamWriter(rutaLibros, false);

                for (int i = 0; i < totalLibros; i++)
                {
                    // formato: codigo,titulo,autor,editorial,anio,categoria,ejemplares
                    string linea = libros[i].Codigo + "," +
                                   libros[i].Titulo + "," +
                                   libros[i].Autor + "," +
                                   libros[i].Editorial + "," +
                                   libros[i].AnioPublicacion + "," +
                                   libros[i].Categoria + "," +
                                   libros[i].EjemplaresDisponibles;
                    sw.WriteLine(linea);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al guardar libros: " + e.Message);
            }
        }

        public static int CargarLibros(Libro[] libros)
        {
            int total = 0;

            try
            {
                if (!File.Exists(rutaLibros))
                    return 0;

                StreamReader sr = new StreamReader(rutaLibros);
                string linea;

                while ((linea = sr.ReadLine()) != null && total < libros.Length)
                {
                    if (string.IsNullOrEmpty(linea))
                        continue;

                    string[] partes = linea.Split(',');

                    // verificar que la linea tenga todos los campos
                    if (partes.Length < 7)
                        continue;

                    libros[total].Codigo = partes[0];
                    libros[total].Titulo = partes[1];
                    libros[total].Autor = partes[2];
                    libros[total].Editorial = partes[3];

                    // capturar excepcion si el año no es numero
                    try
                    {
                        libros[total].AnioPublicacion = int.Parse(partes[4]);
                    }
                    catch
                    {
                        libros[total].AnioPublicacion = 2000;
                    }

                    libros[total].Categoria = partes[5];

                    try
                    {
                        libros[total].EjemplaresDisponibles = int.Parse(partes[6]);
                    }
                    catch
                    {
                        libros[total].EjemplaresDisponibles = 0;
                    }

                    total++;
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al cargar libros: " + e.Message);
            }

            return total;
        }

        // ============================================================
        // USUARIOS - archivo TXT separado por |
        // ============================================================

        public static void GuardarUsuarios(Usuario[] usuarios, int totalUsuarios)
        {
            try
            {
                if (!Directory.Exists("Data"))
                    Directory.CreateDirectory("Data");

                StreamWriter sw = new StreamWriter(rutaUsuarios, false);

                for (int i = 0; i < totalUsuarios; i++)
                {
                    // uso | como separador para evitar problemas con comas en nombres
                    string linea = usuarios[i].Carne + "|" +
                                   usuarios[i].NombreCompleto + "|" +
                                   usuarios[i].Carrera + "|" +
                                   usuarios[i].Correo + "|" +
                                   usuarios[i].Telefono + "|" +
                                   usuarios[i].Estado;
                    sw.WriteLine(linea);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al guardar usuarios: " + e.Message);
            }
        }

        public static int CargarUsuarios(Usuario[] usuarios)
        {
            int total = 0;

            try
            {
                if (!File.Exists(rutaUsuarios))
                    return 0;

                StreamReader sr = new StreamReader(rutaUsuarios);
                string linea;

                while ((linea = sr.ReadLine()) != null && total < usuarios.Length)
                {
                    if (string.IsNullOrEmpty(linea))
                        continue;

                    string[] partes = linea.Split('|');

                    if (partes.Length < 6)
                        continue;

                    usuarios[total].Carne = partes[0];
                    usuarios[total].NombreCompleto = partes[1];
                    usuarios[total].Carrera = partes[2];
                    usuarios[total].Correo = partes[3];
                    usuarios[total].Telefono = partes[4];
                    usuarios[total].Estado = partes[5];

                    total++;
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al cargar usuarios: " + e.Message);
            }

            return total;
        }

        // ============================================================
        // PRESTAMOS - archivo TXT
        // ============================================================

        public static void GuardarPrestamos(Prestamo[] prestamos, int totalPrestamos)
        {
            try
            {
                if (!Directory.Exists("Data"))
                    Directory.CreateDirectory("Data");

                StreamWriter sw = new StreamWriter(rutaPrestamos, false);

                for (int i = 0; i < totalPrestamos; i++)
                {
                    string linea = prestamos[i].IdPrestamo + "|" +
                                   prestamos[i].CarneUsuario + "|" +
                                   prestamos[i].CodigoLibro + "|" +
                                   prestamos[i].FechaPrestamo + "|" +
                                   prestamos[i].FechaDevolucion + "|" +
                                   prestamos[i].Estado;
                    sw.WriteLine(linea);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al guardar prestamos: " + e.Message);
            }
        }

        public static int CargarPrestamos(Prestamo[] prestamos)
        {
            int total = 0;

            try
            {
                if (!File.Exists(rutaPrestamos))
                    return 0;

                StreamReader sr = new StreamReader(rutaPrestamos);
                string linea;

                while ((linea = sr.ReadLine()) != null && total < prestamos.Length)
                {
                    if (string.IsNullOrEmpty(linea))
                        continue;

                    string[] partes = linea.Split('|');

                    if (partes.Length < 6)
                        continue;

                    prestamos[total].IdPrestamo = partes[0];
                    prestamos[total].CarneUsuario = partes[1];
                    prestamos[total].CodigoLibro = partes[2];
                    prestamos[total].FechaPrestamo = partes[3];
                    prestamos[total].FechaDevolucion = partes[4];
                    prestamos[total].Estado = partes[5];

                    total++;
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al cargar prestamos: " + e.Message);
            }

            return total;
        }
    }
}
