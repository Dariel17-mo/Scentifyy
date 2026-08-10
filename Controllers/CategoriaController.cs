using Microsoft.AspNetCore.Mvc;
using Scentify.Models;
using Scentify.Data;

namespace Scentify.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaRepository _repo = new CategoriaRepository();

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
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                int result = _repo.Insert(categoria);
                if (result == 1)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error al guardar la categoría");
            }
            return View(categoria);
        }

        public IActionResult Edit(int id)
        {
            var categoria = _repo.GetById(id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                int result = _repo.Update(categoria);
                if (result == 1)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error al actualizar la categoría");
            }
            return View(categoria);
        }

        public IActionResult Delete(int id)
        {
            var categoria = _repo.GetById(id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int result = _repo.Delete(id);
            if (result == 1)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Error al eliminar la categoría");
            var categoria = _repo.GetById(id);
            return View("Delete", categoria);
        }

    }
}
