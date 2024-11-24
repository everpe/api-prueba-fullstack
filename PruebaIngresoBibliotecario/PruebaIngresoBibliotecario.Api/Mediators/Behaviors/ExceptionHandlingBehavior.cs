using MediatR;
using Microsoft.AspNetCore.Http;
using PruebaIngresoBibliotecario.Api.Utilities;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
namespace PruebaIngresoBibliotecario.Api.Mediators.Behaviors
{

    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next )
        {
            try
            {
                return await next();
            }
            catch (FluentValidation.ValidationException ex)
            {
                // Captura errores de FluentValidation y lanza un 400 con los mensajes de error combinados
                var errorMessage = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                throw new BadHttpRequestException(errorMessage, StatusCodes.Status400BadRequest);
            }
            catch (BusinessException ex)
            {
                throw new BadHttpRequestException(ex.Message, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el servidor.", ex);
            }
        }
    }

}
