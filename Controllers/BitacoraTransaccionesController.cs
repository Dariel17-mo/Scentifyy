using Microsoft.AspNetCore.Mvc;
using Scentify.Data;
using Scentify.Models;

namespace Scentify.Controllers
{
    public class BitacoraTransaccionesController : Controller
    {
        private readonly BitacoraTransaccionesRepository _repository;

        public BitacoraTransaccionesController()
        {
            _repository = new BitacoraTransaccionesRepository();
        }

        public IActionResult Index()
        {
            var lista = _repository.Listar();
            var usuarios = new UsuarioRepository().ListarTodos();

            foreach (var item in lista)
            {
                var usuario = usuarios.FirstOrDefault(u => u.UsuarioID == item.UsuarioID);
                if (usuario != null)
                {
                    item.UsuarioNombre = $"{usuario.Nombre} {usuario.Apellido1}";
                }
            }

            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BitacoraTransacciones transaccion)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(transaccion);
        }

        public IActionResult Details(int id)
        {
            var lista = _repository.Listar();
            var transaccion = lista.FirstOrDefault(x => x.BitacoraTransaccionesID == id);
            if (transaccion == null)
                return NotFound();

            return View(transaccion);
        }

        public IActionResult Edit(int id)
        {
            var lista = _repository.Listar();
            var transaccion = lista.FirstOrDefault(x => x.BitacoraTransaccionesID == id);
            if (transaccion == null)
                return NotFound();

            return View(transaccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BitacoraTransacciones transaccion)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(transaccion);
        }

        public IActionResult Delete(int id)
        {
            var lista = _repository.Listar();
            var transaccion = lista.FirstOrDefault(x => x.BitacoraTransaccionesID == id);
            if (transaccion == null)
                return NotFound();

            return View(transaccion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}