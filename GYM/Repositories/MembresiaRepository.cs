using GYM.Models;
using GymWorkersApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GYM.Repositories
{
    public class MembresiaRepository : IMembresiaRepository
    {
        private readonly ApplicationDbContext _context;

        public MembresiaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Membresia>> ObtenerTodasAsync()
        {
            return await _context.Membresias
                .Include(m => m.Miembro)
                .OrderByDescending(m => m.FechaInicio)
                .ToListAsync();
        }

        public async Task<Membresia?> ObtenerPorIdAsync(int id)
        {
            return await _context.Membresias
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task RegistrarAsync(Membresia membresia)
        {
            _context.Membresias.Add(membresia);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Membresia>> ObtenerPorMiembroIdAsync(int miembroId)
        {
            return await _context.Membresias
                .Include(m => m.Miembro)
                .Where(m => m.MiembroId == miembroId)
                .OrderByDescending(m => m.FechaInicio)
                .ToListAsync();
        }
    }
}
