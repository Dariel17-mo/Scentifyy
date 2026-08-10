using Microsoft.AspNetCore.Mvc;
using Scentify.Models;
using Scentify.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scentify.Repositories;
using Microsoft.AspNetCore.Http;

namespace Scentify.Controllers
{
    public class ResenaController : Controller
    {
        private readonly ResenaRepository _repo;
        private readonly ProductoRepository _productoRepo;
        private readonly BitacoraTransaccionesRepository _bitacoraRepo;

        public ResenaController(ResenaRepository repo, ProductoRepository productoRepo, BitacoraTransaccionesRepository bitacoraRepo)
        {
            _repo = repo;
            _productoRepo = productoRepo;
            _bitacoraRepo = bitacoraRepo;
        }

        public IActionResult Index()
        {
            var lista = _repo.GetAll();
            return View(lista);
        }

        public IActionResult Create()
        {
            var productos = _productoRepo.GetAll();
            ViewBag.Productos = new SelectList(productos, "ProductoID", "Nombre");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Resena resena)
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null)
                return RedirectToAction("Login", "Usuario");

            resena.UsuarioID = usuarioID.Value;

            if (resena.ProductoID == 0 || resena.Calificacion < 1 || resena.Calificacion > 5 || string.IsNullOrWhiteSpace(resena.Comentario))
            {
                ModelState.AddModelError("", "Todos los campos son obligatorios y válidos.");
            }
            else
            {
                int result = _repo.Insert(resena);
                if (result == 1)
                {
                    _bitacoraRepo.RegistrarBitacora("Resena", "INSERT", usuarioID, $"Se creó una reseña para el producto ID {resena.ProductoID} con calificación {resena.Calificacion}");
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al guardar la reseña.");
            }

            var productos = _productoRepo.GetAll();
            ViewBag.Productos = new SelectList(productos, "ProductoID", "Nombre", resena.ProductoID);
            return View(resena);
        }

        public IActionResult Edit(int id)
        {
            var resena = _repo.GetById(id);
            if (resena == null) return NotFound();
            return View(resena);
        }

        [HttpPost]
        public IActionResult Edit(Resena resena)
        {
            var original = _repo.GetById(resena.ResenaID);
            if (original == null)
                return NotFound();

            original.Calificacion = resena.Calificacion;
            original.Comentario = resena.Comentario;

            int result = _repo.Update(original);
            if (result == 1)
            {
                int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
                _bitacoraRepo.RegistrarBitacora("Resena", "UPDATE", usuarioID, $"Se actualizó la reseña ID {original.ResenaID} con calificación {original.Calificacion}");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al actualizar la reseña");
            return View(original);
        }

        public IActionResult Delete(int id)
        {
            var resena = _repo.GetById(id);
            if (resena == null) return NotFound();
            return View(resena);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            int result = _repo.Delete(id);
            if (result == 1)
            {
                int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
                _bitacoraRepo.RegistrarBitacora("Resena", "DELETE", usuarioID, $"Se eliminó la reseña ID {id}");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al eliminar la reseña");
            var resena = _repo.GetById(id);
            return View("Delete", resena);
        }
    }
}
