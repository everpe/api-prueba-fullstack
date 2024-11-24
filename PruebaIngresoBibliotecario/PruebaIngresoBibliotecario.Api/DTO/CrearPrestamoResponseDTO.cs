using System;

namespace PruebaIngresoBibliotecario.Api.DTO
{
    public class CrearPrestamoResponseDTO
    {
        public Guid Id { get; set; } 
        public string FechaMaximaDevolucion { get; set; } = null!; 
    }
}
