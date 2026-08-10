using System;
using System.Collections.Generic;
using Scentify.Models;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Scentify.Repositories
{
    using Scentify.Data;

    public class TransaccionPagoRepository
    {
        public TransaccionPagoRepository(IConfiguration configuration)
        {
        }

        public int GuardarPagoPedido(int pedidoId, string metodoPago, string estado)
        {
            var pedido = MockDatabase.Pedidos.FirstOrDefault(p => p.PedidoID == pedidoId);
            decimal total = pedido?.Total ?? 0m;

            if (pedido != null)
            {
                pedido.Estado = "Pagado";
            }

            int newId = MockDatabase.TransaccionesPago.Any() ? MockDatabase.TransaccionesPago.Max(t => t.TransaccionPagoID) + 1 : 1;
            var pago = new TransaccionPago
            {
                TransaccionPagoID = newId,
                PedidoID = pedidoId,
                MetodoPago = metodoPago,
                Monto = total,
                Estado = estado,
                CodigoTransaccion = "TX_" + new Random().Next(10000000, 99999999),
                FechaCreacion = DateTime.Now
            };

            MockDatabase.TransaccionesPago.Add(pago);
            return 1;
        }

        public List<TransaccionPago> ObtenerTodosLosPagos()
        {
            return MockDatabase.TransaccionesPago.OrderByDescending(t => t.FechaCreacion).ToList();
        }
    }
}
