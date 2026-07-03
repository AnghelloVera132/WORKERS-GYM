using GYM.Models;
using GymWorkersApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GYM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CarritoCompras()
        {
            return View();
        }

        public IActionResult Horarios()
        {
            return View();
        }

        public IActionResult Tienda()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]

        public IActionResult IniciarSesion(string Correo, string Contraseña)
        {
            var usuarioEncontrado = _context.Miembros
                .FirstOrDefault(m => m.Correo == Correo && m.Contraseña == Contraseña);

            if (usuarioEncontrado != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Correo o contraseña incorrectos. ¿Estás seguro que te registraste?";
                return View();
            }
        }
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrarse(Miembro nuevoMiembro)
        {
            var existeCorreo = _context.Miembros.Any(m => m.Correo == nuevoMiembro.Correo);
            if (existeCorreo)
            {
                ViewBag.Error = "Este correo ya está registrado.";
                return View(nuevoMiembro);
            }

            ModelState.Remove("Id");
            ModelState.Remove("FechaRegistro");

            if (ModelState.IsValid)
            {
                nuevoMiembro.FechaRegistro = DateTime.Now;  

                _context.Miembros.Add(nuevoMiembro);
                _context.SaveChanges();

                return RedirectToAction("IniciarSesion");
            }

            var erroresOcultos = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            ViewBag.Error = "Error secreto del sistema: " + string.Join(" | ", erroresOcultos);

            return View(nuevoMiembro);
        }

        public IActionResult PerfilUsu()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
