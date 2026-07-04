using GYM.Models;

namespace GYM.Repositories
{
    public interface IMembresiaRepository
    {
        Task<List<Membresia>> ObtenerTodasAsync();
        Task<Membresia?> ObtenerPorIdAsync(int id);
        Task RegistrarAsync(Membresia membresia);
        Task<List<Membresia>> ObtenerPorMiembroIdAsync(int miembroId);
    }
}
