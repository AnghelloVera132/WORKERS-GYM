using GYM.Models;
using GYM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class MembresiasController : Controller
    {
        private readonly IMembresiaRepository _membresiaRepository;
        private readonly IMiembroRepository _miembroRepository;

        public MembresiasController(
            IMembresiaRepository membresiaRepository,
            IMiembroRepository miembroRepository)
        {
            _membresiaRepository = membresiaRepository;
            _miembroRepository = miembroRepository;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            var membresias =
                await _membresiaRepository.ObtenerTodasAsync();

            return View(membresias);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            string DNI,
            string Tipo,
            DateTime FechaInicio)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            // Buscar miembro por DNI
            var miembro = _miembroRepository.BuscarPorDni(DNI);

            if (miembro == null)
            {
                ViewBag.Error = "No se encontró ningún miembro con ese DNI.";
                ViewBag.DNI = DNI;
                return View();
            }

            // Definir precio y duración automáticamente
            decimal precio;
            DateTime fechaFin;

            switch (Tipo)
            {
                case "Mensual":
                    precio = 60m;
                    fechaFin = FechaInicio.AddMonths(1);
                    break;

                case "Trimestral":
                    precio = 160m;
                    fechaFin = FechaInicio.AddMonths(3);
                    break;

                case "Anual":
                    precio = 600m;
                    fechaFin = FechaInicio.AddYears(1);
                    break;

                default:
                    ViewBag.Error = "Seleccione un tipo de membresía válido.";
                    return View();
            }

            var nuevaMembresia = new Membresia
            {
                MiembroId = miembro.Id,
                Tipo = Tipo,
                Precio = precio,
                FechaInicio = FechaInicio,
                FechaFin = fechaFin,
                Estado = "Activa"
            };

            await _membresiaRepository
                .RegistrarAsync(nuevaMembresia);

            return RedirectToAction(nameof(Index));
        }
    }
}