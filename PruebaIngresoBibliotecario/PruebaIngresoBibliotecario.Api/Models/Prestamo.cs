using System;

namespace PruebaIngresoBibliotecario.Api.Models
{
 
    public class Prestamo
    {
        public Guid IdPrestamo { get; set; } // Identificador único del préstamo
        public Guid IdEjemplar { get; set; } // FK hacia EjemplarLibro
        public string IdentificacionUsuario { get; set; } = null!; // FK hacia Usuario
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaMaximaDevolucion { get; set; }

        // Navegación
        public virtual EjemplarLibro Ejemplar { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }

    
}
