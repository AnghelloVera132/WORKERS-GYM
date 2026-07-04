using GYM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class MiembrosController : Controller
    {
        private readonly IMiembroRepository _miembroRepository;

        public MiembrosController(IMiembroRepository miembroRepository)
        {
            _miembroRepository = miembroRepository;
        }

        // =========================
        // LISTAR Y BUSCAR MIEMBROS
        // =========================
        public async Task<IActionResult> Index(string? dni)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            // Solo Administrador y Recepción pueden entrar
            if (rol != "Administrador" && rol != "Recepcion")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            ViewBag.DNI = dni;

            // Si se escribió un DNI, realizar búsqueda
            if (!string.IsNullOrWhiteSpace(dni))
            {
                var resultado =
                    await _miembroRepository
                        .BuscarPorDniAsync(dni.Trim());

                return View(resultado);
            }

            // Si no hay búsqueda, mostrar todos
            var miembros =
                await _miembroRepository.ObtenerTodosAsync();

            return View(miembros);
        }

        // =========================
        // ELIMINAR MIEMBRO
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rol =
                HttpContext.Session.GetString("UsuarioRol");

            var usuarioId =
                HttpContext.Session.GetInt32("UsuarioId");

            // Solo Administrador puede eliminar
            if (rol != "Administrador")
            {
                TempData["Error"] =
                    "No tienes permisos para eliminar miembros.";

                return RedirectToAction("Index");
            }

            // Evitar que el administrador elimine
            // su propia cuenta conectada
            if (usuarioId.HasValue && usuarioId.Value == id)
            {
                TempData["Error"] =
                    "No puedes eliminar tu propia cuenta mientras tienes la sesión iniciada.";

                return RedirectToAction("Index");
            }

            var eliminado =
                await _miembroRepository.EliminarAsync(id);

            if (!eliminado)
            {
                TempData["Error"] =
                    "No se pudo eliminar el miembro. Puede tener una membresía registrada o el miembro ya no existe.";

                return RedirectToAction("Index");
            }

            TempData["Exito"] =
                "Miembro eliminado correctamente.";

            return RedirectToAction("Index");
        }
    }
}