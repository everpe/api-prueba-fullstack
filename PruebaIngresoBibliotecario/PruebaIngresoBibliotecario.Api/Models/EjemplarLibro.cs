using System;

namespace PruebaIngresoBibliotecario.Api.Models
{
    public class EjemplarLibro
    {
        public Guid IdEjemplar { get; set; } 
        public Guid Isbn { get; set; } 
        public virtual Libro Libro { get; set; } 
        public bool EstaDisponible { get; set; } 
    }
}
