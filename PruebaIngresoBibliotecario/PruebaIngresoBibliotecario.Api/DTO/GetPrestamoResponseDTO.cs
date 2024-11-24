using System;

namespace PruebaIngresoBibliotecario.Api.DTO
{
    public class GetPrestamoByIdResponseDTO
    {
        public Guid Id { get; set; }
        public string Isbn { get; set; } = null!;
        public string IdentificacionUsuario { get; set; } = null!;
        public int TipoUsuario { get; set; }
        public DateTime FechaMaximaDevolucion { get; set; } 
    }

}
