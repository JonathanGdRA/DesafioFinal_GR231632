// Validaciones.cs
// Metodos de validacion para los diferentes campos del sistema
// Hice esta clase separada para no repetir codigo en cada modulo

using System;

namespace BibliotecaUDB
{
    static class Validaciones
    {
        // Valida que el codigo del libro tenga exactamente 8 caracteres alfanumericos
        // ejemplo valido: LIB00001
        public static bool ValidarCodigoLibro(string codigo)
        {
            if (codigo == null || codigo.Length != 8)
                return false;

            // verificar que todos los caracteres sean letras o numeros
            foreach (char c in codigo)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }

        // El carne debe tener exactamente 8 digitos numericos
        public static bool ValidarCarne(string carne)
        {
            if (carne == null || carne.Length != 8)
                return false;

            for (int i = 0; i < carne.Length; i++)
            {
                if (!char.IsDigit(carne[i]))
                    return false;
            }
            return true;
        }

        // validar que el correo tenga @ y un punto despues del @
        public static bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return false;

            int posArroba = correo.IndexOf('@');
            if (posArroba < 0)
                return false;

            // buscar punto despues del @
            string despuesArroba = correo.Substring(posArroba);
            if (despuesArroba.IndexOf('.') < 0)
                return false;

            return true;
        }

        // el año debe estar entre 1900 y el año actual
        public static bool ValidarAnio(int anio)
        {
            int anioActual = DateTime.Now.Year;
            if (anio < 1900 || anio > anioActual)
                return false;
            return true;
        }

        // valida formato de fecha dd/mm/yyyy
        // solo valida la estructura, no si la fecha es real
        public static bool ValidarFecha(string fecha)
        {
            if (fecha == null || fecha.Length != 10)
                return false;

            // verificar que los separadores esten en la posicion correcta
            if (fecha[2] != '/' || fecha[5] != '/')
                return false;

            // intentar parsear las partes
            try
            {
                int dia = int.Parse(fecha.Substring(0, 2));
                int mes = int.Parse(fecha.Substring(3, 2));
                int anio = int.Parse(fecha.Substring(6, 4));

                if (dia < 1 || dia > 31) return false;
                if (mes < 1 || mes > 12) return false;
                if (anio < 1900) return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        // verifica que un string no este vacio
        public static bool NoEstaVacio(string texto)
        {
            return !string.IsNullOrEmpty(texto) && texto.Trim().Length > 0;
        }

        // Valida que la cantidad sea un numero no negativo
        public static bool ValidarCantidad(int cantidad)
        {
            return cantidad >= 0;
        }
    }
}
