using Microsoft.AspNetCore.Mvc;
using Scentify.Data;

namespace TuNamespace.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoRepository _repo = new PedidoRepository();

        // Lista pedidos, con filtro opcional por usuarioId (solo admins)
        public IActionResult Index(int? usuarioId, string nombre, DateTime? fechaInicio, DateTime? fechaFin)
        {
            string perfil = HttpContext.Session.GetString("Perfil")?.ToUpper();
            int? sessionUsuarioId = HttpContext.Session.GetInt32("UsuarioID");

            if (string.IsNullOrEmpty(perfil) || sessionUsuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var pedidos = perfil == "ADMINISTRADOR"
                ? _repo.GetAll()
                : _repo.GetByUsuarioId(sessionUsuarioId.Value);

            // Filtros aplicables
            if (usuarioId.HasValue && perfil == "ADMINISTRADOR")
                pedidos = pedidos.Where(p => p.UsuarioID == usuarioId.Value).ToList();

            if (!string.IsNullOrEmpty(nombre))
                pedidos = pedidos.Where(p => p.UsuarioNombre != null && p.UsuarioNombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();

            if (fechaInicio.HasValue)
                pedidos = pedidos.Where(p => p.FechaPedido.Date >= fechaInicio.Value.Date).ToList();

            if (fechaFin.HasValue)
                pedidos = pedidos.Where(p => p.FechaPedido.Date <= fechaFin.Value.Date).ToList();

            return View(pedidos);
        }
    }
}
