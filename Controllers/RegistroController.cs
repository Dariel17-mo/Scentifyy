using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scentify.Data;
using Scentify.Models;

public class RegistroController : Controller
{
    private readonly UsuarioRepository _repo;

    public RegistroController()
    {
        _repo = new UsuarioRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new Usuario());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(Usuario usuario)
    {
        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        int result = _repo.Insert(usuario);

        if (result == 1)
        {
            return RedirectToAction("Login", "Usuario");
        }

        ModelState.AddModelError("", "No se pudo registrar el usuario.");
        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult Create(Usuario usuario)
    {
        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        int resultado = _repo.Insert(usuario);

        if (resultado == 1)
        {
            return RedirectToAction("Login", "Usuario");
        }

        ModelState.AddModelError("", "No se pudo registrar el usuario. Intente nuevamente.");
        return View(usuario);
    }

}
