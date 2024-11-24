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
                var errorMessage = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                throw new CustomHttpException(new { mensaje = errorMessage }, StatusCodes.Status400BadRequest);
            }
            catch (BusinessException ex)
            {
                throw new CustomHttpException(new { mensaje = ex.Message }, ex.StatusCode);
            }
            catch (Exception ex)
            {
                throw new CustomHttpException(new { mensaje = "Ocurrió un error interno en el servidor." }, StatusCodes.Status500InternalServerError);
            }

        }
    }

    public class CustomHttpException : Exception
    {
        public object Response { get; }
        public int StatusCode { get; }

        public CustomHttpException(object response, int statusCode)
        {
            Response = response;
            StatusCode = statusCode;
        }
    }


}
