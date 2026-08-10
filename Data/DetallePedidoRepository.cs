using System.Collections.Generic;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class DetallePedidoRepository
    {
        public void Insert(DetallePedido detalle, Microsoft.Data.SqlClient.SqlConnection conn, Microsoft.Data.SqlClient.SqlTransaction transaction)
        {
            int newId = MockDatabase.DetallesPedido.Any() ? MockDatabase.DetallesPedido.Max(d => d.DetallePedidoID) + 1 : 1;
            detalle.DetallePedidoID = newId;

            var product = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == detalle.ProductoID);
            detalle.ProductoNombre = product?.Nombre;
            detalle.ImagenURL = product?.ImagenURL;
            detalle.Subtotal = (detalle.PrecioUnitario - detalle.Descuento) * detalle.Cantidad;

            MockDatabase.DetallesPedido.Add(detalle);
        }

        public List<DetallePedido> GetAll()
        {
            return MockDatabase.DetallesPedido;
        }

        public List<DetallePedido> GetByPedidoId(int pedidoId)
        {
            return MockDatabase.DetallesPedido.Where(d => d.PedidoID == pedidoId).ToList();
        }
    }
}
