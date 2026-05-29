// Structs.cs
// Aqui defino todas las estructuras de datos que voy a usar en el sistema
// Cada struct representa una entidad del sistema de biblioteca

namespace BibliotecaUDB
{
    // Estructura para representar un libro
    struct Libro
    {
        public string Codigo;       // formato LIB00001
        public string Titulo;
        public string Autor;
        public string Editorial;
        public int AnioPublicacion;
        public string Categoria;
        public int EjemplaresDisponibles;
    }

    // Estructura para representar un usuario de la biblioteca
    struct Usuario
    {
        public string Carne;           // 8 digitos numericos
        public string NombreCompleto;
        public string Carrera;
        public string Correo;
        public string Telefono;
        public string Estado;          // "activo" o "inactivo"
    }

    // Estructura para los prestamos
    struct Prestamo
    {
        public string IdPrestamo;       // identificador unico
        public string CarneUsuario;
        public string CodigoLibro;
        public string FechaPrestamo;    // formato dd/mm/yyyy
        public string FechaDevolucion;  // fecha estimada de devolucion
        public string Estado;           // "activo" o "devuelto"
    }
}
