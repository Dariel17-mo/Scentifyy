using System;
using System.Collections.Generic;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class CarritoCompraRepository
    {
        public List<CarritoCompra> GetByUsuario(int usuarioId)
        {
            var lista = MockDatabase.Carritos.Where(c => c.UsuarioID == usuarioId).ToList();
            foreach (var item in lista)
            {
                var prod = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == item.ProductoID);
                if (prod != null)
                {
                    item.ProductoNombre = prod.Nombre;
                    item.Precio = prod.Precio;
                    item.Descuento = prod.Descuento;
                    item.ImagenURL = prod.ImagenURL;
                    item.Subtotal = (prod.Precio - prod.Descuento) * item.Cantidad;
                }
            }
            return lista;
        }

        public int AgregarProducto(int usuarioId, int productoId, int cantidad)
        {
            var existing = MockDatabase.Carritos.FirstOrDefault(c => c.UsuarioID == usuarioId && c.ProductoID == productoId);
            if (existing != null)
            {
                existing.Cantidad += cantidad;
                existing.FechaActualizacion = DateTime.Now;
            }
            else
            {
                int newId = MockDatabase.Carritos.Any() ? MockDatabase.Carritos.Max(c => c.CarritoCompraID) + 1 : 1;
                MockDatabase.Carritos.Add(new CarritoCompra
                {
                    CarritoCompraID = newId,
                    UsuarioID = usuarioId,
                    ProductoID = productoId,
                    Cantidad = cantidad,
                    FechaAgregado = DateTime.Now
                });
            }
            return 1;
        }

        public int ActualizarProducto(int usuarioId, int productoId, int nuevaCantidad)
        {
            var existing = MockDatabase.Carritos.FirstOrDefault(c => c.UsuarioID == usuarioId && c.ProductoID == productoId);
            if (existing != null)
            {
                existing.Cantidad = nuevaCantidad;
                existing.FechaActualizacion = DateTime.Now;
                return 1;
            }
            return -1;
        }

        public int EliminarProducto(int usuarioId, int productoId)
        {
            var existing = MockDatabase.Carritos.FirstOrDefault(c => c.UsuarioID == usuarioId && c.ProductoID == productoId);
            if (existing != null)
            {
                MockDatabase.Carritos.Remove(existing);
                return 1;
            }
            return -1;
        }

        public int ConfirmarCarrito(int usuarioId, string direccionEnvio, string telefonoContacto = null, string notas = null)
        {
            var cart = GetByUsuario(usuarioId);
            if (!cart.Any()) return -1;

            int newPedidoId = MockDatabase.Pedidos.Any() ? MockDatabase.Pedidos.Max(p => p.PedidoID) + 1 : 1;
            
            var user = MockDatabase.Usuarios.FirstOrDefault(u => u.UsuarioID == usuarioId);
            string userNombre = user != null ? $"{user.Nombre} {user.Apellido1}" : "Cliente Anónimo";
            string userEmail = user?.Email ?? "";

            decimal total = cart.Sum(c => c.Subtotal);

            // Crear Pedido
            var order = new Pedido
            {
                PedidoID = newPedidoId,
                UsuarioID = usuarioId,
                UsuarioNombre = userNombre,
                Email = userEmail,
                DireccionEnvio = direccionEnvio,
                TelefonoContacto = telefonoContacto,
                Total = total,
                FechaPedido = DateTime.Now,
                Estado = "Pendiente de Pago",
                Notas = notas
            };
            MockDatabase.Pedidos.Add(order);

            // Crear Detalles del Pedido
            foreach (var item in cart)
            {
                int newDetailId = MockDatabase.DetallesPedido.Any() ? MockDatabase.DetallesPedido.Max(d => d.DetallePedidoID) + 1 : 1;
                MockDatabase.DetallesPedido.Add(new DetallePedido
                {
                    DetallePedidoID = newDetailId,
                    PedidoID = newPedidoId,
                    ProductoID = item.ProductoID,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Precio,
                    Descuento = item.Descuento,
                    Subtotal = item.Subtotal,
                    ProductoNombre = item.ProductoNombre,
                    ImagenURL = item.ImagenURL
                });

                // Reducir stock del producto
                var prod = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == item.ProductoID);
                if (prod != null)
                {
                    prod.Stock = Math.Max(0, prod.Stock - item.Cantidad);
                }
            }

            // Limpiar el carrito del usuario
            MockDatabase.Carritos.RemoveAll(c => c.UsuarioID == usuarioId);

            // Registrar en la bitácora de transacciones
            int newLogId = MockDatabase.BitacoraTransacciones.Any() ? MockDatabase.BitacoraTransacciones.Max(b => b.BitacoraTransaccionesID) + 1 : 1;
            MockDatabase.BitacoraTransacciones.Add(new BitacoraTransacciones
            {
                BitacoraTransaccionesID = newLogId,
                Tabla = "Pedido",
                Accion = "INSERT",
                UsuarioID = usuarioId,
                Fecha = DateTime.Now,
                Descripcion = $"Se registró el pedido con ID {newPedidoId} desde el carrito de compras."
            });

            return newPedidoId;
        }
    }
}
