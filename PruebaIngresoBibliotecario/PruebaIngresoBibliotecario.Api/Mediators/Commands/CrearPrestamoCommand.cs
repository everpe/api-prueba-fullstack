using FluentValidation;
using MediatR;
using PruebaIngresoBibliotecario.Api.DTO;
using PruebaIngresoBibliotecario.Api.Models;
using System;
using System.Threading.Tasks;
using System.Threading;
using PruebaIngresoBibliotecario.Api.Interfaces;

namespace PruebaIngresoBibliotecario.Api.Mediators.Commands
{
    public class CrearPrestamoCommand : IRequest<CrearPrestamoResponseDTO>
    {
        public Guid Isbn { get; set; } 
        public string IdentificacionUsuario { get; set; } = null!; 
        public TipoUsuario TipoUsuario { get; set; } 
    }

    public class CrearPrestamoCommandValidator : AbstractValidator<CrearPrestamoCommand>
    {
        public CrearPrestamoCommandValidator()
        {
            RuleFor(x => x.Isbn)
                .NotEmpty().WithMessage("El ISBN es requerido.");

            RuleFor(x => x.IdentificacionUsuario)
                .NotEmpty()
                .WithMessage("La identificación del usuario es requerida.")
                .Length(1, 10)
                .WithMessage("La identificación debe tener entre 1 y 10 caracteres.");

            RuleFor(x => x.TipoUsuario)
                .IsInEnum()
                .WithMessage("El tipo de usuario debe ser 1 (Afiliado), 2 (Empleado), o 3 (Invitado).");
        }

       
    }

    public class CrearPrestamoHandler : IRequestHandler<CrearPrestamoCommand, CrearPrestamoResponseDTO>
    {
        private readonly IProductService _productService;

        public CrearPrestamoHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<CrearPrestamoResponseDTO> Handle(CrearPrestamoCommand request, CancellationToken cancellationToken)
        {
            return await _productService.CrearPrestamoAsync(request, cancellationToken); 
        }

    }
}
