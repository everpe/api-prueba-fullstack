using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Api.Models
{
    public class Libro
    {
        public Guid Isbn { get; set; }
        [MaxLength(100)]
        public string Titulo { get; set; } = null!;
        [MaxLength(100)]
        public string Autor { get; set; } = null!;
        public int AnioPublicacion { get; set; }
        public ICollection<EjemplarLibro> Ejemplares { get; set; } = new List<EjemplarLibro>();
    }
}
