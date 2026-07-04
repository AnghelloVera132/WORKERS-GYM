using GYM.Models;

namespace GYM.Repositories
{
    public interface IMiembroRepository
    {
        Miembro? BuscarPorCorreoYContraseña(
            string correo,
            string contraseña
        );

        Miembro? BuscarPorDni(string dni);

        bool ExisteCorreo(string correo);

        void Registrar(Miembro miembro);

        Task<List<Miembro>> ObtenerTodosAsync();

        Task<List<Miembro>> BuscarPorDniAsync(string dni);
        Task<bool> EliminarAsync(int id);
    }
}