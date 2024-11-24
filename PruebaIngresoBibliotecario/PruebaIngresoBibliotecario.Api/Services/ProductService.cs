using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PruebaIngresoBibliotecario.Api.DTO;
using PruebaIngresoBibliotecario.Api.Infraestructure;
using PruebaIngresoBibliotecario.Api.Interfaces;
using PruebaIngresoBibliotecario.Api.Mediators.Commands;
using PruebaIngresoBibliotecario.Api.Models;
using PruebaIngresoBibliotecario.Api.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly PersistenceContext _context;
        private readonly ILogger<ProductService> _logger;
        public ProductService(PersistenceContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<CrearPrestamoResponseDTO> CrearPrestamoAsync(CrearPrestamoCommand request, CancellationToken cancellationToken) 
        {
            await ValidarExistenciaUsuario(request.IdentificacionUsuario, cancellationToken);
            await ValidarExistenciaIsbn(request.Isbn, cancellationToken);
            await ValidarPrestamoExistenteUsuarioInvitado(request.TipoUsuario, request.IdentificacionUsuario, cancellationToken);

            var prestamo = new Prestamo
            {
                IdPrestamo = Guid.NewGuid(),
                IdEjemplar = await ObtenerEjemplarDisponible(request.Isbn),
                IdentificacionUsuario = request.IdentificacionUsuario,
                TipoUsuario = request.TipoUsuario,
                FechaPrestamo = DateTime.UtcNow,
                FechaMaximaDevolucion = CalcularFechaEntrega(request.TipoUsuario),
                EstaActivo = true
            };

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync(cancellationToken);

            return new CrearPrestamoResponseDTO
            {
                Id = prestamo.IdPrestamo,
                FechaMaximaDevolucion = prestamo.FechaMaximaDevolucion.ToString("dd/MM/yyyy")
            };
        }




        private async Task<Guid> ObtenerEjemplarDisponible(Guid isbn)
        {
            var ejemplar = await _context.Ejemplares
                .FirstOrDefaultAsync(e => e.Isbn == isbn && e.EstaDisponible);

            if (ejemplar == null)
            {
                throw new BusinessException("No hay ejemplares disponibles para este libro.");
            }

            ejemplar.EstaDisponible = false;
            return ejemplar.IdEjemplar;
        }

        private DateTime CalcularFechaEntrega(TipoUsuario tipoUsuario)
        {
            // Determina los días hábiles adicionales según el tipo de usuario
            int diasHabiles = tipoUsuario switch
            {
                TipoUsuario.AFILIADO => 10,
                TipoUsuario.EMPLEADO => 8,
                TipoUsuario.INVITADO => 7,
                _ => throw new ArgumentOutOfRangeException(nameof(tipoUsuario), "Tipo de usuario no válido.")
            };

            return SumarDiasHabiles(DateTime.UtcNow, diasHabiles);
        }

        private DateTime SumarDiasHabiles(DateTime fechaInicial, int diasHabiles)
        {
            var fechaActual = fechaInicial;

            while (diasHabiles > 0)
            {
                fechaActual = fechaActual.AddDays(1);

                // Verifica si el día es hábil (lunes a viernes)
                if (fechaActual.DayOfWeek != DayOfWeek.Saturday && fechaActual.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasHabiles--;
                }
            }

            return fechaActual;
        }


        public async Task ValidarPrestamoExistenteUsuarioInvitado(TipoUsuario tipoUsuario, string identificacionUsuario, CancellationToken cancellation)
        {

            if (tipoUsuario == Models.TipoUsuario.INVITADO)
            {
                var tienePrestamoActivo = await _context.Prestamos
                    .AnyAsync(p => p.IdentificacionUsuario == identificacionUsuario &&
                                   p.EstaActivo, cancellation);
                if (tienePrestamoActivo)
                {
                    throw new BusinessException(
                        $"El usuario con identificacion {identificacionUsuario} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo"
                    );
                }
            }
        }

        private async Task ValidarExistenciaIsbn(Guid isbn, CancellationToken cancellationToken)
        {
            var existeLibro = await _context.Libros.AnyAsync(l => l.Isbn == isbn, cancellationToken);
            if (!existeLibro)
            {
                throw new BusinessException($"No existe un libro con el ISBN: {isbn}");
            }
        }
        private async Task ValidarExistenciaUsuario(string identificacionUsuario, CancellationToken cancellationToken)
        {
            var existeUsuario = await _context.Usuarios.AnyAsync(u => u.IdentificacionUsuario == identificacionUsuario, cancellationToken);
            if (!existeUsuario)
            {
                throw new BusinessException($"No existe un usuario con la identificación: {identificacionUsuario}");
            }
        }



        private async Task ConcluirPrestamoAsync(Guid idPrestamo)
        {
            var prestamo = await _context.Prestamos
                .FirstOrDefaultAsync(p => p.IdPrestamo == idPrestamo);

            if (prestamo == null)
                throw new Exception("Préstamo no encontrado.");

            // Marcar el préstamo como concluido
            prestamo.EstaActivo = false;

            // Liberar el ejemplar
            var ejemplar = await _context.Ejemplares
                .FirstOrDefaultAsync(e => e.IdEjemplar == prestamo.IdEjemplar);

            if (ejemplar != null)
                ejemplar.EstaDisponible = true;

            await _context.SaveChangesAsync();
        }

    }
}
