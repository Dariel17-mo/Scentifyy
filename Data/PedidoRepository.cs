using System;
using System.Collections.Generic;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class PedidoRepository
    {
        public List<Pedido> GetAll()
        {
            return MockDatabase.Pedidos;
        }

        public Pedido GetById(int pedidoId)
        {
            return MockDatabase.Pedidos.FirstOrDefault(p => p.PedidoID == pedidoId);
        }

        public List<Pedido> GetByUsuarioId(int usuarioId)
        {
            return MockDatabase.Pedidos.Where(p => p.UsuarioID == usuarioId).ToList();
        }
    }
}
