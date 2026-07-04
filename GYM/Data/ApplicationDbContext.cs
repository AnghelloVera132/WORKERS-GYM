using GYM.Models;
using Microsoft.EntityFrameworkCore;

namespace GymWorkersApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas de la base de datos
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }

        // Configuración de los modelos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Precio de membresía
            modelBuilder.Entity<Membresia>()
                .Property(m => m.Precio)
                .HasPrecision(10, 2);

            // Monto del pago
            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasPrecision(10, 2);
        }
    }
}