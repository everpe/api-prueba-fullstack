using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaIngresoBibliotecario.Api.Mediators.Commands;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using PruebaIngresoBibliotecario.Api.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PruebaIngresoBibliotecario.Api.Infraestructure;
using PruebaIngresoBibliotecario.Api.Utilities;
using PruebaIngresoBibliotecario.Api.Mediators.Querys;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PersistenceContext _context;


        public PrestamoController(IMediator mediator)
        {
            _mediator = mediator;
        }




        /// <summary>
        /// Permite la creación de nuevos prestamos de libros a Usuarios.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CrearPrestamo([FromBody] CrearPrestamoCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        /// <summary>
        /// Permite listar los prestamos creados
        /// </summary>
        /// <param name="idPrestamo"></param>
        /// <returns></returns>
        [HttpGet("{idPrestamo}")]
        public async Task<IActionResult> GetPrestamoById(Guid idPrestamo)
        {
            var query = new GetPrestamoQuery { IdPrestamo = idPrestamo };
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
