using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scentify.Data;
using Scentify.Models;
using Scentify.Repositories;
using X.PagedList;


namespace Scentify.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoRepository _repo;
        private readonly ResenaRepository _resenaRepo;
        private readonly BitacoraTransaccionesRepository _bitacoraRepo;
        private readonly MarcaRepository _marcaRepo;
        private readonly CategoriaRepository _categoriaRepo;
        private readonly UsuarioRepository _usuarioRepo;

        public ProductoController(ProductoRepository repo, ResenaRepository resenaRepo, BitacoraTransaccionesRepository bitacoraRepo, MarcaRepository marcaRepo, CategoriaRepository categoriaRepo, UsuarioRepository usuarioRepo)
        {
            _repo = repo;
            _resenaRepo = resenaRepo;
            _bitacoraRepo = bitacoraRepo;
            _marcaRepo = marcaRepo;
            _categoriaRepo = categoriaRepo;
            _usuarioRepo = usuarioRepo;
        }

        // Catálogo
        public IActionResult Index(string? marca, string? orden, string? categoria, decimal? precioMin, decimal? precioMax, bool? soloStock, int? page)
        {
            int pageSize = 8;
            int pageNumber = page ?? 1;

            var productos = _repo.GetFiltrados(marca, categoria, precioMin, precioMax, soloStock, orden);

            ViewBag.Marcas = _marcaRepo.GetAll().Select(m => m.Nombre).Distinct().ToList();
            ViewBag.Categorias = _categoriaRepo.GetAll().Select(c => c.Nombre).Distinct().ToList();

            ViewBag.MarcaSeleccionada = marca;
            ViewBag.CategoriaSeleccionada = categoria;
            ViewBag.OrdenActual = orden;
            ViewBag.PrecioMin = precioMin;
            ViewBag.PrecioMax = precioMax;
            ViewBag.SoloStock = soloStock;

            var pagedProductos = productos.ToPagedList(pageNumber, pageSize);

            string correoUsuario = HttpContext.Session.GetString("UsuarioEmail");
            if (!string.IsNullOrEmpty(correoUsuario))
            {
                var usuario = _usuarioRepo.GetByEmail(correoUsuario);
                if (usuario != null)
                {
                    ViewBag.UsuarioRol = usuario.Rol;
                }
            }

            return View(pagedProductos);
        }




        // Crear Producto
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_categoriaRepo.GetAll(), "CategoriaID", "Nombre");
            ViewBag.Marcas = new SelectList(_marcaRepo.GetAll(), "MarcaID", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            try
            {
                int result = _repo.Insert(producto);
                if (result == 1)
                {
                    int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
                    _bitacoraRepo.RegistrarBitacora("Producto", "INSERT", usuarioID, $"Se creó el producto '{producto.Nombre}' con precio {producto.Precio}");
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al guardar el producto en la base de datos.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Excepción: {ex.Message}");
            }

            ViewBag.Categorias = new SelectList(_categoriaRepo.GetAll(), "CategoriaID", "Nombre", producto.CategoriaID);
            ViewBag.Marcas = new SelectList(_marcaRepo.GetAll(), "MarcaID", "Nombre", producto.MarcaID);
            return View(producto);
        }

        // Editar Producto
        public IActionResult Edit(int id)
        {
            var producto = _repo.GetById(id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = new SelectList(_categoriaRepo.GetAll(), "CategoriaID", "Nombre", producto.CategoriaID);
            ViewBag.Marcas = new SelectList(_marcaRepo.GetAll(), "MarcaID", "Nombre", producto.MarcaID);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ModelState.AddModelError("", "Errores: " + string.Join(", ", errores));
            }
            else
            {
                int result = _repo.Update(producto);
                if (result == 1)
                {
                    int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
                    _bitacoraRepo.RegistrarBitacora("Producto", "UPDATE", usuarioID, $"Se actualizó el producto '{producto.Nombre}' con ID {producto.ProductoID}");
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al actualizar el producto");
            }

            ViewBag.Categorias = new SelectList(_categoriaRepo.GetAll(), "CategoriaID", "Nombre", producto.CategoriaID);
            ViewBag.Marcas = new SelectList(_marcaRepo.GetAll(), "MarcaID", "Nombre", producto.MarcaID);
            return View(producto);
        }

        // Eliminar Producto
        public IActionResult Delete(int id)
        {
            var producto = _repo.GetById(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var producto = _repo.GetById(id);
            int result = _repo.Delete(id);
            if (result == 1)
            {
                int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
                _bitacoraRepo.RegistrarBitacora("Producto", "DELETE", usuarioID, $"Se eliminó el producto '{producto?.Nombre}' con ID {id}");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al eliminar el producto");
            return View("Delete", producto);
        }

        // Detalle Producto
        public IActionResult Detalle(int id, string? error = null)
        {
            var producto = _repo.GetById(id);
            if (producto == null)
                return NotFound();

            if (!string.IsNullOrEmpty(error))
                ViewBag.ErrorMensaje = error;

            return View(producto);
        }

        // Agregar Reseña
        [HttpPost]
        public IActionResult AgregarResena(Resena resena)
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null)
                return RedirectToAction("Login", "Usuario");

            resena.UsuarioID = usuarioID.Value;

            if (resena.ProductoID == 0 || resena.Calificacion < 1 || resena.Calificacion > 5 || string.IsNullOrWhiteSpace(resena.Comentario))
            {
                return RedirectToAction("Detalle", new { id = resena.ProductoID, error = "Datos inválidos. Por favor revise el formulario." });
            }

            int resultado = _resenaRepo.Insert(resena);

            if (resultado == 1)
            {
                _bitacoraRepo.RegistrarBitacora("Resena", "INSERT", usuarioID, $"Se agregó una reseña al producto ID {resena.ProductoID} con calificación {resena.Calificacion}");
                return RedirectToAction("Detalle", new { id = resena.ProductoID });
            }
            else
            {
                return RedirectToAction("Detalle", new { id = resena.ProductoID, error = "Error al guardar la reseña. Intente nuevamente." });
            }
        }
    }
}
