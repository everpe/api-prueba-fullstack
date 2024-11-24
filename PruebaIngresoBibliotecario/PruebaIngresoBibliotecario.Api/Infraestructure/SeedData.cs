using PruebaIngresoBibliotecario.Api.Models;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Infraestructure
{
    public static class SeedData
    {
        public static async Task SeedAsync(PersistenceContext context)
        {
            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(
                        new Usuario { Nombre = "Ever", IdentificacionUsuario = "AFILIADO01", TipoUsuario = TipoUsuario.AFILIADO },
                        new Usuario { Nombre = "Pedro", IdentificacionUsuario = "EMPLEADO01", TipoUsuario = TipoUsuario.EMPLEADO },
                        new Usuario { Nombre = "Armando", IdentificacionUsuario = "123456789", TipoUsuario = TipoUsuario.INVITADO }
                );
            }

            if (!context.Libros.Any())
            {
                var libro1 = new Libro { Isbn = Guid.NewGuid(), Titulo = "El Principito", Autor = "Autor 1" };
                var libro2 = new Libro { Isbn = Guid.NewGuid(), Titulo = "La Divina Comedia", Autor = "Autor 2" };

                context.Libros.AddRange(libro1, libro2);

                context.Ejemplares.AddRange(
                    new EjemplarLibro { IdEjemplar = Guid.NewGuid(), Isbn = libro1.Isbn, EstaDisponible = true },
                    new EjemplarLibro { IdEjemplar = Guid.NewGuid(), Isbn = libro1.Isbn, EstaDisponible = true },
                    new EjemplarLibro { IdEjemplar = Guid.NewGuid(), Isbn = libro2.Isbn, EstaDisponible = true },
                    new EjemplarLibro { IdEjemplar = Guid.NewGuid(), Isbn = libro2.Isbn, EstaDisponible = true }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
