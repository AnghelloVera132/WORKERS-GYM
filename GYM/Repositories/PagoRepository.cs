using GYM.Models;
using GymWorkersApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GYM.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly ApplicationDbContext _context;

        public PagoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pago>> ObtenerTodosAsync()
        {
            return await _context.Pagos
                .Include(p => p.Membresia)
                    .ThenInclude(m => m.Miembro)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        public async Task<Pago?> ObtenerPorIdAsync(int id)
        {
            return await _context.Pagos
                .Include(p => p.Membresia)
                    .ThenInclude(m => m.Miembro)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task RegistrarAsync(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Pago>> ObtenerPorMiembroIdAsync(int miembroId)
        {
            return await _context.Pagos
                .Include(p => p.Membresia)
                    .ThenInclude(m => m.Miembro)
                .Where(p => p.Membresia != null && p.Membresia.MiembroId == miembroId)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }
    }
}