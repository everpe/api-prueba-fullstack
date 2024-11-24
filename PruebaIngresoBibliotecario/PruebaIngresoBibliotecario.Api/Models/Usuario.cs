using System.Collections.Generic;

namespace PruebaIngresoBibliotecario.Api.Models
{
    public class Usuario
    {
        public string IdentificacionUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public TipoUsuario TipoUsuario { get; set; }
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
    public enum TipoUsuario { AFILIADO = 1, EMPLEADO, INVITADO }
}
