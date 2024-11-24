﻿using MediatR;
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


        public PrestamoController(IMediator mediator, PersistenceContext context)
        {
            _mediator = mediator;
            _context = context;
        }



        [HttpGet]
        [Route("/libros")]
        public async Task<List<Libro>> Libros()
        {
            return await _context.Libros.ToListAsync();
        }


        [HttpGet]
        [Route("/usuarios")]
        public async Task<List<Usuario>> Usuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet]
        [Route("/lista")]
        public async Task<List<Prestamo>> LIstado()
        {
            return await _context.Prestamos.ToListAsync();
        }

        /// <summary>
        /// Permite la creación de nuevos prestamos de libros a Usuarios.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CrearPrestamo([FromBody] CrearPrestamoCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error interno en el servidor.", detalle = ex.Message });
            }
        }



        [HttpGet("{idPrestamo:guid}")]
        public async Task<IActionResult> GetPrestamoById(Guid idPrestamo)
        {
            try
            {
                var query = new GetPrestamoQuery { IdPrestamo = idPrestamo };
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (BusinessException ex) when (ex.StatusCode == 404)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }
    }
}
