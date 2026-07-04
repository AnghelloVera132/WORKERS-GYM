using GYM.Models;

namespace GYM.Repositories
{
    public interface IPagoRepository
    {
        Task<List<Pago>> ObtenerTodosAsync();
        Task<Pago?> ObtenerPorIdAsync(int id);
        Task RegistrarAsync(Pago pago);
        Task<List<Pago>> ObtenerPorMiembroIdAsync(int miembroId);
    }
}