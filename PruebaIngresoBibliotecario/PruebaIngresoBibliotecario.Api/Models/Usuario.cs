using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Api.Models
{
    public class Usuario
    {
        [MaxLength(10)]
        public string IdentificacionUsuario { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;
        public TipoUsuario TipoUsuario { get; set; }
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
    public enum TipoUsuario { AFILIADO = 1, EMPLEADO, INVITADO }
}
