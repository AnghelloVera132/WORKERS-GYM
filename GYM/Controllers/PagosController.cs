using GYM.Models;
using GYM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class PagosController : Controller
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IMembresiaRepository _membresiaRepository;

        public PagosController(
            IPagoRepository pagoRepository,
            IMembresiaRepository membresiaRepository)
        {
            _pagoRepository = pagoRepository;
            _membresiaRepository = membresiaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
                return RedirectToAction("IniciarSesion", "Home");

            var pagos = await _pagoRepository.ObtenerTodosAsync();

            return View(pagos);
        }

        [HttpGet]
        public async Task<IActionResult> Crear(int membresiaId)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
                return RedirectToAction("IniciarSesion", "Home");

            var membresia = await _membresiaRepository.ObtenerPorIdAsync(membresiaId);

            if (membresia == null)
                return RedirectToAction("Index", "Membresias");

            ViewBag.Membresia = membresia;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(int MembresiaId, string MetodoPago)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
                return RedirectToAction("IniciarSesion", "Home");

            var membresia = await _membresiaRepository.ObtenerPorIdAsync(MembresiaId);

            if (membresia == null)
            {
                ViewBag.Error = "No se encontró la membresía.";
                return View();
            }

            var pago = new Pago
            {
                MembresiaId = membresia.Id,
                Monto = membresia.Precio,
                FechaPago = DateTime.Now,
                MetodoPago = MetodoPago,
                Estado = "Pagado"
            };

            await _pagoRepository.RegistrarAsync(pago);

            return RedirectToAction(nameof(Index));
        }
    }
}