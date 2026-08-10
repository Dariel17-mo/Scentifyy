using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scentify.Data;
using Scentify.Models;
using Scentify.Repositories;
using Microsoft.Extensions.Configuration;
using Scentify.Services;
using System.Threading.Tasks;

namespace Scentify.Controllers
{
    public class CarritoCompraController : Controller
    {
        private readonly CarritoCompraRepository _repo;
        private readonly TransaccionPagoRepository _transaccionRepo;
        private readonly IEmailService _emailService;
        private readonly ProductoRepository _productoRepo;
        private readonly UsuarioRepository _usuarioRepo;
        private readonly PedidoRepository _pedidoRepo;

        public CarritoCompraController(IConfiguration configuration, IEmailService emailService, ProductoRepository productoRepo, UsuarioRepository usuarioRepo, PedidoRepository pedidoRepo)
        {
            _repo = new CarritoCompraRepository();
            _transaccionRepo = new TransaccionPagoRepository(configuration);
            _emailService = emailService;
            _productoRepo = productoRepo;
            _usuarioRepo = usuarioRepo;
            _pedidoRepo = pedidoRepo;
        }

        private int UsuarioId => HttpContext.Session.GetInt32("UsuarioID") ?? 0;

        public IActionResult Index()
        {
            var carrito = _repo.GetByUsuario(UsuarioId);
            return View(carrito);
        }

        [HttpPost]
        public IActionResult Agregar(int productoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                return BadRequest("Cantidad debe ser mayor a cero");
            }

            int resultado = _repo.AgregarProducto(UsuarioId, productoId, cantidad);
            if (resultado == 1)
                return RedirectToAction("Index");

            return BadRequest("Error al agregar producto al carrito");
        }

        [HttpPost]
        public IActionResult Actualizar(int productoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                TempData["Error"] = "La cantidad debe ser mayor a cero.";
                return RedirectToAction("Index");
            }

            var producto = _productoRepo.GetById(productoId);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("Index");
            }

            if (cantidad > producto.Stock)
            {
                TempData["Error"] = $"Stock insuficiente. Solo hay {producto.Stock} unidades disponibles.";
                return RedirectToAction("Index");
            }

            int resultado = _repo.ActualizarProducto(UsuarioId, productoId, cantidad);
            if (resultado == 1)
            {
                TempData["Mensaje"] = "Cantidad actualizada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Error al actualizar la cantidad del producto.";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Eliminar(int productoId)
        {
            int resultado = _repo.EliminarProducto(UsuarioId, productoId);
            if (resultado == 1)
                return RedirectToAction("Index");

            return BadRequest("Error al eliminar producto del carrito");
        }

        [HttpPost]
        public IActionResult AgregarAjax(int productoId, int cantidad)
        {
            var producto = _productoRepo.GetById(productoId);
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            if (producto.Stock <= 0)
            {
                return Json(new { success = false, message = "Este producto no tiene stock disponible." });
            }

            if (cantidad > producto.Stock)
            {
                return Json(new { success = false, message = $"Solo hay {producto.Stock} unidades disponibles." });
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
            {
                return Json(new { success = false, message = "Usuario no autenticado." });
            }

            var resultado = _repo.AgregarProducto(usuarioId.Value, productoId, cantidad);

            if (resultado == 1)
            {
                return Json(new { success = true });
            }
            else if (resultado == -1)
            {
                return Json(new { success = false, message = "Producto ya en el carrito." });
            }
            else if (resultado == -2)
            {
                return Json(new { success = false, message = "No hay suficiente stock." });
            }
            else
            {
                return Json(new { success = false, message = "Error al agregar el producto al carrito." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Confirmar(string direccionEnvio, string telefonoContacto, string notas, string metodoPago)
        {
            if (string.IsNullOrWhiteSpace(direccionEnvio))
                return BadRequest("Debe ingresar una dirección de envío");

            try
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioID");
                if (usuarioId == null)
                    return Unauthorized("No se encontró sesión del usuario");

                int pedidoId = _repo.ConfirmarCarrito(usuarioId.Value, direccionEnvio, telefonoContacto, notas);

                if (metodoPago == "Tarjeta")
                {
                    int resPago = _transaccionRepo.GuardarPagoPedido(pedidoId, "Tarjeta", "Aprobado");

                    var usuarioObj = _usuarioRepo.GetById(usuarioId.Value);
                    var pedidoObj = _pedidoRepo.GetById(pedidoId);

                    if (usuarioObj != null && pedidoObj != null)
                    {
                        string nombreCompleto = $"{usuarioObj.Nombre} {usuarioObj.Apellido1}";
                        await _emailService.EnviarComprobanteAsync(usuarioObj.Email, nombreCompleto, pedidoId, pedidoObj.Total, metodoPago, "Aprobado", "Pedido-" + pedidoId);
                    }

                    TempData["Mensaje"] = $"¡Compra confirmada con éxito y su pago verificado por tarjeta! Pedido #{pedidoId}. Revisa tu correo.";
                }
                else if (metodoPago == "SINPE MOVIL")
                {
                    TempData["Mensaje"] = "Su pedido ha sido reservado. Para que sea procesado, recuerde enviar su SINPE MÓVIL al 6398-6348 con el detalle de su pedido.";
                }
                else if (metodoPago == "PayPal")
                {
                    return Redirect("https://www.paypal.com/cr/home");
                }
                else if (metodoPago == "Stripe")
                {
                    return Redirect("https://stripe.com/es-us");
                }
                else
                {
                    TempData["Mensaje"] = "¡Compra confirmada con éxito!";
                }

                return RedirectToAction("Index", "Producto");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al confirmar el carrito: " + ex.Message);
            }
        }
    }
}
