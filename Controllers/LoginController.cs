using Microsoft.AspNetCore.Mvc;
using Scentify.Models;
using Scentify.Data;
using Scentify.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Scentify.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioRepository _repo;
        private readonly IEmailService _emailService;

        public LoginController(IEmailService emailService, UsuarioRepository repo)
        {
            _repo = repo;
            _emailService = emailService;
        }

        // GET: /Login
        public IActionResult Index()
        {
            return View(new Login());
        }

        // POST: /Login
        [HttpPost]
        public IActionResult Index(Login model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int respuesta;
            var usuario = _repo.ValidarLogin(model.Email, model.Contrasena, out respuesta);

            if (respuesta == 1 && usuario != null)
            {
                // Guardar datos del usuario en sesión
                HttpContext.Session.SetInt32("UsuarioID", usuario.UsuarioID);
                HttpContext.Session.SetString("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido1} {usuario.Apellido2}");
                HttpContext.Session.SetString("Rol", usuario.Rol);
                HttpContext.Session.SetString("Email", usuario.Email);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RecuperarPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Por favor, ingresa tu correo electrónico.";
                return View();
            }

            var usuario = _repo.GetByEmail(email);
            if (usuario == null)
            {
                ViewBag.Error = "No se encontró ninguna cuenta asociada a este correo.";
                return View();
            }

            try
            {
                string nombreCompleto = $"{usuario.Nombre} {usuario.Apellido1}";
                await _emailService.EnviarRecuperacionDeContrasenaAsync(usuario.Email, nombreCompleto, usuario.Contrasena);
                ViewBag.Exito = "¡Te hemos enviado un correo con tu contraseña! Revisa tu bandeja de entrada o carpeta de Spam.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Hubo un problema al enviar el correo: " + ex.Message;
            }

            return View();
        }
    }
}
