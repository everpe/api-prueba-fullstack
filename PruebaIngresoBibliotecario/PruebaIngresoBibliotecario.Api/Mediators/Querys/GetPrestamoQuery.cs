using MediatR;
using PruebaIngresoBibliotecario.Api.DTO;
using PruebaIngresoBibliotecario.Api.Infraestructure;
using PruebaIngresoBibliotecario.Api.Utilities;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PruebaIngresoBibliotecario.Api.Interfaces;

namespace PruebaIngresoBibliotecario.Api.Mediators.Querys
{
    public class GetPrestamoQuery : IRequest<GetPrestamoByIdResponseDTO>
    {
        public Guid IdPrestamo { get; set; }
    }

    public class GetPrestamoByIdHandler : IRequestHandler<GetPrestamoQuery, GetPrestamoByIdResponseDTO>
    {
        private readonly IProductService _productService;

        public GetPrestamoByIdHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetPrestamoByIdResponseDTO> Handle(GetPrestamoQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetPrestamosByIdAsync(request, cancellationToken);
        }
    }
}
