using Microsoft.AspNetCore.Mvc;
using Scentify.Data;
using Scentify.Models;

namespace TuProyecto.Controllers
{
    public class DetallePedidoController : Controller
    {
        private readonly DetallePedidoRepository _repo = new DetallePedidoRepository();

        // Acción para listar todos los detalles de pedidos
        public IActionResult Index()
        {
            var lista = _repo.GetAll();
            return View(lista);
        }

        // Acción para mostrar detalles de un pedido específico
        public IActionResult DetallesPorPedido(int pedidoId)
        {
            var detalles = _repo.GetByPedidoId(pedidoId);

            if (detalles == null || detalles.Count == 0)
            {
                ViewBag.Mensaje = "Este pedido no contiene productos.";
                return View("DetallesPorPedido", new List<DetallePedido>()); // Mostrar vista vacía con mensaje
            }

            return View("DetallesPorPedido", detalles);
        }

    }
}
