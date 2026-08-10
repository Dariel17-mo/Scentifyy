using System.Collections.Generic;
using Scentify.Models;
using System.Linq;
using System;

namespace Scentify.Data
{
    public class ClienteDashboardRepository
    {
        public List<Pedido> GetUltimosPedidos(int usuarioId)
        {
            return MockDatabase.Pedidos
                .Where(p => p.UsuarioID == usuarioId)
                .OrderByDescending(p => p.FechaPedido)
                .Take(5)
                .ToList();
        }

        public List<ProductoCompradoEstadistica> GetProductosMasComprados(int usuarioId)
        {
            var pedidosIds = MockDatabase.Pedidos.Where(p => p.UsuarioID == usuarioId).Select(p => p.PedidoID).ToList();
            return MockDatabase.DetallesPedido
                .Where(d => pedidosIds.Contains(d.PedidoID))
                .GroupBy(d => d.ProductoNombre)
                .Select(g => new ProductoCompradoEstadistica
                {
                    Producto = g.Key,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(s => s.Cantidad)
                .ToList();
        }

        public (int TotalPedidos, int EnProceso, int TotalProductosComprados) GetEstadisticasGenerales(int usuarioId)
        {
            var pedidos = MockDatabase.Pedidos.Where(p => p.UsuarioID == usuarioId).ToList();
            int totalPedidos = pedidos.Count;
            int enProceso = pedidos.Count(p => p.Estado != "Entregado" && p.Estado != "Cancelado" && p.Estado != "Pagado");
            
            var pedidosIds = pedidos.Select(p => p.PedidoID).ToList();
            int totalProductosComprados = MockDatabase.DetallesPedido
                .Where(d => pedidosIds.Contains(d.PedidoID))
                .Sum(d => d.Cantidad);

            return (totalPedidos, enProceso, totalProductosComprados);
        }
    }
}
