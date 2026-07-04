using GYM.Models;
using GymWorkersApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GYM.Repositories
{
    public class AsistenciaRepository : IAsistenciaRepository
    {
        private readonly ApplicationDbContext _context;

        public AsistenciaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Asistencia>> ObtenerTodasAsync()
        {
            return await _context.Asistencias
                .Include(a => a.Miembro)
                .OrderByDescending(a => a.FechaHoraEntrada)
                .ToListAsync();
        }

        public async Task RegistrarAsync(Asistencia asistencia)
        {
            _context.Asistencias.Add(asistencia);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsistenciaHoyAsync(int miembroId)
        {
            var inicioHoy = DateTime.Today;
            var inicioManana = inicioHoy.AddDays(1);

            return await _context.Asistencias
                .AnyAsync(a =>
                    a.MiembroId == miembroId &&
                    a.FechaHoraEntrada >= inicioHoy &&
                    a.FechaHoraEntrada < inicioManana
                );
        }

        public async Task<List<Asistencia>> FiltrarAsync(
            string? dni,
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            var consulta = _context.Asistencias
                .Include(a => a.Miembro)
                .AsQueryable();

            // Filtrar por DNI si fue ingresado
            if (!string.IsNullOrWhiteSpace(dni))
            {
                var dniLimpio = dni.Trim();

                consulta = consulta.Where(a =>
                    a.Miembro != null &&
                    a.Miembro.DNI == dniLimpio
                );
            }

            // Filtrar desde una fecha
            if (fechaDesde.HasValue)
            {
                var desde = fechaDesde.Value.Date;

                consulta = consulta.Where(a =>
                    a.FechaHoraEntrada >= desde
                );
            }

            // Filtrar hasta una fecha incluyendo todo ese día
            if (fechaHasta.HasValue)
            {
                var hastaExclusivo =
                    fechaHasta.Value.Date.AddDays(1);

                consulta = consulta.Where(a =>
                    a.FechaHoraEntrada < hastaExclusivo
                );
            }

            return await consulta
                .OrderByDescending(a => a.FechaHoraEntrada)
                .ToListAsync();
        }
        public async Task<List<Asistencia>> ObtenerPorMiembroIdAsync(int miembroId)
        {
            return await _context.Asistencias
                .Include(a => a.Miembro)
                .Where(a => a.MiembroId == miembroId)
                .OrderByDescending(a => a.FechaHoraEntrada)
                .ToListAsync();
        }
    }
}