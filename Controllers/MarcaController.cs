using Microsoft.AspNetCore.Mvc;
using Scentify.Models;
using Scentify.Data;

namespace Scentify.Controllers
{
    public class MarcaController : Controller
    {
        private readonly MarcaRepository _repo = new MarcaRepository();

        // Listar todas las marcas
        public IActionResult Index()
        {
            var marcas = _repo.GetAll();
            return View(marcas);
        }

        // Mostrar formulario para crear nueva marca
        public IActionResult Create()
        {
            return View();
        }

        // Procesar creación de nueva marca
        [HttpPost]
        public IActionResult Create(Marca marca)
        {
            if (ModelState.IsValid)
            {
                var resultado = _repo.Insert(marca);
                if (resultado == 1)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error al guardar la marca.");
            }
            return View(marca);
        }

        // Mostrar formulario para editar marca
        public IActionResult Edit(int id)
        {
            var marca = _repo.GetById(id);
            if (marca == null) return NotFound();
            return View(marca);
        }

        // Procesar edición de marca
        [HttpPost]
        public IActionResult Edit(Marca marca)
        {
            if (ModelState.IsValid)
            {
                var resultado = _repo.Update(marca);
                if (resultado == 1)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error al actualizar la marca.");
            }
            return View(marca);
        }

        // Confirmación para eliminar marca
        public IActionResult Delete(int id)
        {
            var marca = _repo.GetById(id);
            if (marca == null) return NotFound();
            return View(marca);
        }

        // Procesar eliminación de marca
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var resultado = _repo.Delete(id);
            if (resultado == 1)
                return RedirectToAction("Index");

            // Si falla, puedes redirigir a la lista con un mensaje de error o manejarlo distinto
            return RedirectToAction("Delete", new { id = id });
        }
    }
}
