using GYM.Models;

namespace GYM.Repositories
{
    public interface IAsistenciaRepository
    {
        Task<List<Asistencia>> ObtenerTodasAsync();

        Task RegistrarAsync(Asistencia asistencia);

        Task<bool> ExisteAsistenciaHoyAsync(int miembroId);

        Task<List<Asistencia>> FiltrarAsync(
            string? dni,
            DateTime? fechaDesde,
            DateTime? fechaHasta
        );
        Task<List<Asistencia>> ObtenerPorMiembroIdAsync(int miembroId);
    }
}