using GYM.Models;
using GymWorkersApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GYM.Repositories
{
    public class MiembroRepository : IMiembroRepository
    {
        private readonly ApplicationDbContext _context;

        public MiembroRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Miembro? BuscarPorCorreoYContraseña(
            string correo,
            string contraseña)
        {
            return _context.Miembros
                .FirstOrDefault(m =>
                    m.Correo == correo &&
                    m.Contraseña == contraseña);
        }

        public Miembro? BuscarPorDni(string dni)
        {
            return _context.Miembros
                .FirstOrDefault(m => m.DNI == dni);
        }

        public bool ExisteCorreo(string correo)
        {
            return _context.Miembros
                .Any(m =>
                    m.Correo.ToLower() == correo.ToLower());
        }

        public void Registrar(Miembro miembro)
        {
            _context.Miembros.Add(miembro);
            _context.SaveChanges();
        }

        public async Task<List<Miembro>> ObtenerTodosAsync()
        {
            return await _context.Miembros
                .OrderBy(m => m.Nombre)
                .ThenBy(m => m.Apellido)
                .ToListAsync();
        }

        public async Task<List<Miembro>> BuscarPorDniAsync(string dni)
        {
            return await _context.Miembros
                .Where(m => m.DNI.Contains(dni))
                .OrderBy(m => m.Nombre)
                .ThenBy(m => m.Apellido)
                .ToListAsync();
        }
        public async Task<bool> EliminarAsync(int id)
        {
            var miembro = await _context.Miembros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (miembro == null)
            {
                return false;
            }

            var tieneMembresias = await _context.Membresias
                .AnyAsync(m => m.MiembroId == id);

            if (tieneMembresias)
            {
                return false;
            }

            _context.Miembros.Remove(miembro);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}