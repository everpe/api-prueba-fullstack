using PruebaIngresoBibliotecario.Api.DTO;
using PruebaIngresoBibliotecario.Api.Mediators.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Interfaces
{
    public interface IProductService
    {
        Task<CrearPrestamoResponseDTO> CrearPrestamoAsync(CrearPrestamoCommand request, CancellationToken cancellationToken);
    }
}
