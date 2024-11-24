using System;
using System.Text.Json.Serialization;

namespace PruebaIngresoBibliotecario.Api.DTO
{
    public class CrearPrestamoResponseDTO
    {
        public Guid Id { get; set; }

        [JsonPropertyName("fechaMaximaDevolucion")]
        public string FechaMaximaDevolucion => FechaMaximaDevolucionDate.ToString("MM/dd/yyyy");//fomrato para q pase test en plataforma

        [JsonIgnore]
        public DateTime FechaMaximaDevolucionDate { get; set; }
    }
}
