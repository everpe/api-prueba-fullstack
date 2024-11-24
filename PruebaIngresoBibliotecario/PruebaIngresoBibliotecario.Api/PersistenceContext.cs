using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PruebaIngresoBibliotecario.Api.Models;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Infrastructure
{
    public class PersistenceContext : DbContext
    {

        private readonly IConfiguration Config;

        public DbSet<Libro> Libros { get; set; }
        public DbSet<EjemplarLibro> Ejemplares { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        public PersistenceContext(DbContextOptions<PersistenceContext> options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Config.GetValue<string>("PrestamosSchema"));

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Libro>(entity =>
            {
                entity.HasKey(l => l.Isbn);
                entity.Property(l => l.Titulo).IsRequired();
                entity.Property(l => l.Autor).IsRequired();
            });

            modelBuilder.Entity<EjemplarLibro>(entity =>
            {
                entity.HasKey(e => e.IdEjemplar);
                entity.HasOne(e => e.Libro)
                      .WithMany(l => l.Ejemplares)
                      .HasForeignKey(e => e.Isbn)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.EstaDisponible).IsRequired();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.IdentificacionUsuario);
                entity.Property(u => u.IdentificacionUsuario)
                      .IsRequired()
                      .HasMaxLength(10);
                entity.Property(u => u.Nombre).IsRequired();
                entity.Property(u => u.TipoUsuario)
                      .IsRequired();
                      //.HasDefaultValue(1); // Afiliado por defecto

                entity.HasIndex(u => u.IdentificacionUsuario)
                      .IsUnique()
                      .HasDatabaseName("IX_Usuario_IdentificacionUnica");
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.HasKey(p => p.IdPrestamo);
                entity.HasOne(p => p.Ejemplar)
                      .WithMany()
                      .HasForeignKey(p => p.IdEjemplar)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.Prestamos)
                      .HasForeignKey(p => p.IdentificacionUsuario)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.Property(p => p.FechaPrestamo).IsRequired();
                entity.Property(p => p.FechaMaximaDevolucion).IsRequired();
            });
        }
    }
}
