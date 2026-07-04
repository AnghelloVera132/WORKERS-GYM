using GYM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GYM.Repositories;

namespace GYM.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMiembroRepository _miembroRepository;
        private readonly IMembresiaRepository _membresiaRepository;
        private readonly IPagoRepository _pagoRepository;
        private readonly IAsistenciaRepository _asistenciaRepository;

        public HomeController(
            IMiembroRepository miembroRepository,
            IMembresiaRepository membresiaRepository,
            IPagoRepository pagoRepository,
            IAsistenciaRepository asistenciaRepository)
        {
            _miembroRepository = miembroRepository;
            _membresiaRepository = membresiaRepository;
            _pagoRepository = pagoRepository;
            _asistenciaRepository = asistenciaRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Horarios()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            return View();
        }

        // =========================
        // INICIAR SESIÓN - GET
        // =========================
        public IActionResult IniciarSesion()
        {
            return View();
        }

        // =========================
        // INICIAR SESIÓN - POST
        // =========================
        [HttpPost]
        public IActionResult IniciarSesion(string Correo, string Contraseña)
        {
            Correo = Correo.Trim().ToLower();

            var usuarioEncontrado =
                _miembroRepository.BuscarPorCorreoYContraseña(
                    Correo,
                    Contraseña
                );

            // Comprobar si el usuario existe
            if (usuarioEncontrado == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            // Guardar datos del usuario en sesión
            HttpContext.Session.SetString(
                "UsuarioCorreo",
                usuarioEncontrado.Correo
            );

            HttpContext.Session.SetString(
                "UsuarioRol",
                usuarioEncontrado.Rol
            );

            HttpContext.Session.SetString(
                "UsuarioNombre",
                usuarioEncontrado.Nombre
            );

            HttpContext.Session.SetInt32(
                "UsuarioId",
                usuarioEncontrado.Id
            );

            // Redirección según el rol
            if (usuarioEncontrado.Rol == "Administrador")
            {
                return RedirectToAction("PanelAdmin");
            }

            if (usuarioEncontrado.Rol == "Recepcion")
            {
                return RedirectToAction("PanelRecepcion");
            }

            return RedirectToAction("PerfilUsu");
        }

        // =========================
        // REGISTRARSE - GET
        // =========================
        public IActionResult Registrarse()
        {
            return View();
        }

        // =========================
        // REGISTRARSE - POST
        // =========================
        [HttpPost]
        public IActionResult Registrarse(Miembro nuevoMiembro)
        {
            nuevoMiembro.Correo =
                nuevoMiembro.Correo.Trim().ToLower();

            var existeCorreo =
                _miembroRepository.ExisteCorreo(
                    nuevoMiembro.Correo
                );

            if (existeCorreo)
            {
                ViewBag.Error = "Este correo ya está registrado.";
                return View(nuevoMiembro);
            }

            // Todo registro público será Usuario
            nuevoMiembro.Rol = "Usuario";

            ModelState.Remove("Id");
            ModelState.Remove("FechaRegistro");
            ModelState.Remove("Rol");

            if (ModelState.IsValid)
            {
                nuevoMiembro.FechaRegistro = DateTime.Now;

                _miembroRepository.Registrar(nuevoMiembro);

                return RedirectToAction("IniciarSesion");
            }

            var errores = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            ViewBag.Error =
                "No se pudo completar el registro: "
                + string.Join(" | ", errores);

            return View(nuevoMiembro);
        }

        // =========================
        // PERFIL DEL USUARIO
        // =========================
        public async Task<IActionResult> PerfilUsu()
        {
            var rol =
                HttpContext.Session.GetString("UsuarioRol");

            var usuarioId =
                HttpContext.Session.GetInt32("UsuarioId");

            // Solo usuarios normales
            if (rol != "Usuario" || usuarioId == null)
            {
                return RedirectToAction("IniciarSesion");
            }

            // Obtener membresías del usuario conectado
            var membresias =
                await _membresiaRepository
                    .ObtenerPorMiembroIdAsync(usuarioId.Value);

            // Obtener pagos del usuario conectado
            var pagos =
                await _pagoRepository
                    .ObtenerPorMiembroIdAsync(usuarioId.Value);
            var asistencias = 
                await _asistenciaRepository
                .ObtenerPorMiembroIdAsync(usuarioId.Value);

            // Enviar información a la vista
            ViewBag.Membresias = membresias;
            ViewBag.Pagos = pagos;
            ViewBag.Asistencias = asistencias;

            return View();
        }

        // =========================
        // PANEL ADMINISTRADOR
        // =========================
        public IActionResult PanelAdmin()
        {
            var rol =
                HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Administrador")
            {
                return RedirectToAction("IniciarSesion");
            }

            return View();
        }

        // =========================
        // PANEL RECEPCIÓN
        // =========================
        public IActionResult PanelRecepcion()
        {
            var rol =
                HttpContext.Session.GetString("UsuarioRol");

            if (rol != "Recepcion")
            {
                return RedirectToAction("IniciarSesion");
            }

            return View();
        }

        // =========================
        // CERRAR SESIÓN
        // =========================
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("IniciarSesion");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // =========================
        // ERROR
        // =========================
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}