using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scentify.Data;
using Scentify.Repositories;
using Scentify.Services;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace Scentify.Controllers
{
    public class TransaccionPagoController : Controller
    {
        private readonly TransaccionPagoRepository _repo;
        private readonly UsuarioRepository _usuarioRepo;
        private readonly PedidoRepository _pedidoRepo;
        private readonly IEmailService _email;

        public TransaccionPagoController(TransaccionPagoRepository repo, UsuarioRepository usuarioRepo, PedidoRepository pedidoRepo, IEmailService email)
        {
            _repo = repo;
            _usuarioRepo = usuarioRepo;
            _pedidoRepo = pedidoRepo;
            _email = email;
        }

        // Index - Ver todos los pagos
        public IActionResult Index()
        {
            var correoUsuario = HttpContext.Session.GetString("UsuarioEmail");
            var usuario = _usuarioRepo.GetByEmail(correoUsuario);

            var pagos = _repo.ObtenerTodosLosPagos();

            // Filtrar pagos por usuario si es cliente
            if (usuario != null && usuario.Rol == "Cliente")
            {
                pagos = pagos.Where(p => p.Pedido != null && p.Pedido.UsuarioID == usuario.UsuarioID).ToList();
            }

            return View(pagos);
        }

        // Mostrar formulario para registrar un pago
        public IActionResult Create()
        {
            ViewBag.Pedidos = _pedidoRepo.GetAll()
                .Where(p => p.Estado != "Pagado")
                .Select(p => new
                {
                    PedidoID = p.PedidoID,
                    Descripcion = $"{p.PedidoID} - {p.UsuarioNombre} (₡{p.Total:N2})",
                    Total = p.Total
                })
                .ToList();

            ViewBag.MetodosPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Efectivo", Text = "Efectivo" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia" },
                new SelectListItem { Value = "Sinpe Móvil", Text = "Sinpe Móvil" },
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta" },
                new SelectListItem { Value = "PAYPAL/STRIPE", Text = "PAYPAL/STRIPE" }
            };

            return View();
        }

        private void RecargarCombos()
        {
            ViewBag.Pedidos = _pedidoRepo.GetAll()
                .Where(p => p.Estado != "Pagado")
                .Select(p => new
                {
                    PedidoID = p.PedidoID,
                    Descripcion = $"{p.PedidoID} - {p.UsuarioNombre} (₡{p.Total:N2})",
                    Total = p.Total
                })
                .ToList();

            ViewBag.MetodosPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Efectivo", Text = "Efectivo" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia" },
                new SelectListItem { Value = "Sinpe Móvil", Text = "Sinpe Móvil" },
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta" },
                new SelectListItem { Value = "PAYPAL/STRIPE", Text = "PAYPAL/STRIPE" }
            };
        }

        // Guardar pago y enviar comprobante por correo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int pedidoId, string metodoPago, string numeroTarjeta, string icvv, string fechaVencimiento)
        {
            if (pedidoId <= 0 || string.IsNullOrWhiteSpace(metodoPago))
            {
                ModelState.AddModelError("", "Datos de pago incompletos o inválidos.");
                RecargarCombos();
                return View();
            }

            // Validación para pagos con tarjeta
            if (metodoPago == "Tarjeta")
            {
                if (string.IsNullOrWhiteSpace(numeroTarjeta) || string.IsNullOrWhiteSpace(icvv) || string.IsNullOrWhiteSpace(fechaVencimiento))
                {
                    ModelState.AddModelError("", "Debe ingresar todos los datos de la tarjeta.");
                    RecargarCombos();
                    return View();
                }

                var pedido = _pedidoRepo.GetById(pedidoId);
                var usuario = _usuarioRepo.GetById(pedido.UsuarioID);

                var pago = new
                {
                    Nombre = usuario.Nombre,
                    NumeroTarjeta = numeroTarjeta,
                    Icvv = icvv,
                    FechaVencimiento = fechaVencimiento,
                    Monto = pedido.Total
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://pagosapi-b8op.onrender.com/");
                    var json = JsonSerializer.Serialize(pago);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("pago", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Error"] = "El pago con tarjeta fue rechazado por la pasarela.";
                        RecargarCombos();
                        return View();
                    }
                }
            }

            string estado = "Aprobado"; // Estado del pago
            int resultado = _repo.GuardarPagoPedido(pedidoId, metodoPago, estado);

            if (resultado == 1)
            {
                // Obtener detalles del pedido y usuario para el comprobante
                var pedido = _pedidoRepo.GetById(pedidoId);
                var usuario = _usuarioRepo.GetById(pedido.UsuarioID);

                // Obtener código de transacción
                string? codigoTransaccion = _repo.ObtenerTodosLosPagos()
                    .Where(t => t.PedidoID == pedidoId)
                    .OrderByDescending(t => t.FechaCreacion)
                    .Select(t => t.CodigoTransaccion)
                    .FirstOrDefault();

                try
                {
                    // Enviar comprobante por correo
                    await _email.EnviarComprobanteAsync(
                        destinatarioEmail: usuario.Email,
                        destinatarioNombre: usuario.Nombre,
                        pedidoId: pedido.PedidoID,
                        monto: pedido.Total,
                        metodoPago: metodoPago,
                        estado: estado,
                        codigoTransaccion: codigoTransaccion
                    );
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al enviar correo: {ex.Message} | {ex.InnerException?.Message}";
                }



                TempData["Mensaje"] = "Pago registrado exitosamente.";
                return RedirectToAction("Index", "Pedido");
            }
            else if (resultado == -2)
            {
                ModelState.AddModelError("", "El pedido no existe.");
            }
            else
            {
                ModelState.AddModelError("", "Error al registrar el pago.");
            }

            RecargarCombos();
            return View();
        }
    }
}
