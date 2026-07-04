using GYM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IMiembroRepository _miembroRepository;
        private readonly IMembresiaRepository _membresiaRepository;
        private readonly IPagoRepository _pagoRepository;

        public ReportesController(
            IMiembroRepository miembroRepository,
            IMembresiaRepository membresiaRepository,
            IPagoRepository pagoRepository)
        {
            _miembroRepository = miembroRepository;
            _membresiaRepository = membresiaRepository;
            _pagoRepository = pagoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            // Solo el Administrador puede ver reportes
            if (rol != "Administrador")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            // Obtener información desde los repositorios
            var miembros = await _miembroRepository.ObtenerTodosAsync();
            var membresias = await _membresiaRepository.ObtenerTodasAsync();
            var pagos = await _pagoRepository.ObtenerTodosAsync();

            // Totales generales
            ViewBag.TotalMiembros = miembros.Count;

            ViewBag.MembresiasActivas = membresias.Count(m =>
                m.FechaFin.Date >= DateTime.Today);

            ViewBag.TotalPagos = pagos.Count;

            ViewBag.IngresosTotales = pagos
                .Where(p => p.Estado == "Pagado")
                .Sum(p => p.Monto);

            // Últimos 5 pagos
            ViewBag.UltimosPagos = pagos
                .OrderByDescending(p => p.FechaPago)
                .Take(5)
                .ToList();

            return View();
        }
    }
}