using System.Collections.Generic;
using System;

namespace PruebaIngresoBibliotecario.Api.Models
{
    public class Libro
    {
        public Guid Isbn { get; set; } 
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public int AnioPublicacion { get; set; }
        public ICollection<EjemplarLibro> Ejemplares { get; set; } = new List<EjemplarLibro>();
    }
}
