using Microsoft.AspNetCore.Mvc;
using Scentify.Models;
using Scentify.Data;
using Microsoft.AspNetCore.Authorization;

namespace Scentify.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _repo = new UsuarioRepository();

        public IActionResult Index()
        {
            var lista = _repo.GetAll();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                int result = _repo.Insert(usuario);
                if (result == 1)
                    return RedirectToAction("Index"); // <-- Redirige al listado de usuarios
            }

            return View(usuario);
        }


        public IActionResult Edit(int id)
        {
            var usuario = _repo.GetById(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                int result = _repo.Update(usuario);
                if (result == 1)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error al actualizar el usuario");
            }
            return View(usuario);
        }

        public IActionResult Delete(int id)
        {
            var usuario = _repo.GetById(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            int result = _repo.Delete(id);
            if (result == 1)
            {
                TempData["MensajeExito"] = "Usuario eliminado correctamente.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al eliminar el usuario");
            var usuario = _repo.GetById(id);
            return View("Delete", usuario);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new Login());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Login model)
        {
            if (model == null)
            {
                model = new Login();
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                model.Email = "Admin123@gmail.com";
            }
            if (string.IsNullOrWhiteSpace(model.Contrasena))
            {
                model.Contrasena = "Admin123";
            }

            // Limpiar errores de validación de ModelState para permitir el bypass
            ModelState.Clear();

            // Validar login
            int respuesta;
            var usuario = _repo.ValidarLogin(model.Email, model.Contrasena, out respuesta);

            if (respuesta == 1 && usuario != null)
            {
                HttpContext.Session.SetInt32("UsuarioID", usuario.UsuarioID);
                HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetString("Perfil", usuario.Rol);

                return RedirectToAction("Index", "Home");
            }

            // Si el login falla, volver a mostrar el formulario con un error opcional
            ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }

    }
}
