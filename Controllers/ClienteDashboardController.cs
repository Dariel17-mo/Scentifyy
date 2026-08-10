using Microsoft.AspNetCore.Mvc;
using Scentify.Data;
using Scentify.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

public class ClienteDashboardController : Controller
{
    private readonly UsuarioRepository _usuarioRepo;
    private readonly ClienteDashboardRepository _dashboardRepo;

    public ClienteDashboardController(UsuarioRepository usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
        _dashboardRepo = new ClienteDashboardRepository();
    }

    public IActionResult Index()
    {
        string correoUsuario = HttpContext.Session.GetString("UsuarioEmail");

        if (string.IsNullOrEmpty(correoUsuario))
            return RedirectToAction("Login", "Usuarios");

        var usuario = _usuarioRepo.GetByEmail(correoUsuario);

        if (usuario == null)
            return RedirectToAction("Login", "Usuarios");

        var pedidos = _dashboardRepo.GetUltimosPedidos(usuario.UsuarioID);
        var productosMasComprados = _dashboardRepo.GetProductosMasComprados(usuario.UsuarioID);
        var stats = _dashboardRepo.GetEstadisticasGenerales(usuario.UsuarioID);

        ViewBag.Pedidos = pedidos;
        ViewBag.MasComprados = productosMasComprados;
        ViewBag.NombreUsuario = usuario.Nombre;
        ViewBag.TotalPedidos = stats.TotalPedidos;
        ViewBag.EnProceso = stats.EnProceso;
        ViewBag.TotalProductosComprados = stats.TotalProductosComprados;

        return View();
    }
}
