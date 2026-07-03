using GYM.Models;
using Microsoft.EntityFrameworkCore;

namespace GymWorkersApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Aquí le decimos a SQL Server qué tablas debe crear
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Rutina> Rutinas { get; set; }
    }
}