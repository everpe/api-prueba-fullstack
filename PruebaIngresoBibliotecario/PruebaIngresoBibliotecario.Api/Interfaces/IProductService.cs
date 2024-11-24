using PruebaIngresoBibliotecario.Api.DTO;
using PruebaIngresoBibliotecario.Api.Mediators.Commands;
using PruebaIngresoBibliotecario.Api.Mediators.Querys;
using System.Threading;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Interfaces
{
    public interface IProductService
    {
        Task<CrearPrestamoResponseDTO> CrearPrestamoAsync(CrearPrestamoCommand request, CancellationToken cancellationToken);
        Task<GetPrestamoByIdResponseDTO> GetPrestamosByIdAsync(GetPrestamoQuery request, CancellationToken cancellationToken);
    }
}
