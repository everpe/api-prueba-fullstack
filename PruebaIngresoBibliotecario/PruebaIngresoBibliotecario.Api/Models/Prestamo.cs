using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Api.Models
{
    public class Prestamo
    {
        public Guid IdPrestamo { get; set; } // Identificador único del préstamo
        public Guid IdEjemplar { get; set; } // FK hacia EjemplarLibro
        [MaxLength(10)]
        public string IdentificacionUsuario { get; set; } = null!; // FK hacia Usuario
        public TipoUsuario TipoUsuario { get; set; } //para el historico
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaMaximaDevolucion { get; set; }
        public bool EstaActivo { get; set; }

        // Navegación
        public virtual EjemplarLibro Ejemplar { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
