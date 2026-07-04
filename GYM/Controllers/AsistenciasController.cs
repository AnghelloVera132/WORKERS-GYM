using GYM.Models;
using GYM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly IAsistenciaRepository _asistenciaRepository;
        private readonly IMiembroRepository _miembroRepository;

        public AsistenciasController(
            IAsistenciaRepository asistenciaRepository,
            IMiembroRepository miembroRepository)
        {
            _asistenciaRepository = asistenciaRepository;
            _miembroRepository = miembroRepository;
        }

        public async Task<IActionResult> Index(
            string? dni,
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            ViewBag.DNI = dni;
            ViewBag.FechaDesde = fechaDesde?.ToString("yyyy-MM-dd");
            ViewBag.FechaHasta = fechaHasta?.ToString("yyyy-MM-dd");

            var asistencias = await _asistenciaRepository
                .FiltrarAsync(dni, fechaDesde, fechaHasta);

            return View(asistencias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(string DNI)
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador" && rol != "Recepcion")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            if (string.IsNullOrWhiteSpace(DNI))
            {
                TempData["Error"] = "Debe ingresar un DNI.";
                return RedirectToAction("Index");
            }

            var miembro = _miembroRepository.BuscarPorDni(DNI.Trim());

            if (miembro == null)
            {
                TempData["Error"] = "No se encontró ningún miembro con ese DNI.";
                return RedirectToAction("Index");
            }

            var yaRegistroHoy = await _asistenciaRepository
                .ExisteAsistenciaHoyAsync(miembro.Id);

            if (yaRegistroHoy)
            {
                TempData["Error"] = "Este miembro ya registró asistencia el día de hoy.";
                return RedirectToAction("Index");
            }

            var asistencia = new Asistencia
            {
                MiembroId = miembro.Id,
                FechaHoraEntrada = DateTime.Now
            };

            await _asistenciaRepository.RegistrarAsync(asistencia);

            TempData["Exito"] = "Asistencia registrada correctamente.";

            return RedirectToAction("Index");
        }
    }
}