using System;

namespace PruebaIngresoBibliotecario.Api.Utilities
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }

}
