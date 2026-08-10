using System;
using System.Collections.Generic;
using Scentify.Models;

namespace Scentify.Data
{
    public static class MockDatabase
    {
        public static List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public static List<Producto> Productos { get; set; } = new List<Producto>();
        public static List<Categoria> Categorias { get; set; } = new List<Categoria>();
        public static List<Marca> Marcas { get; set; } = new List<Marca>();
        public static List<CarritoCompra> Carritos { get; set; } = new List<CarritoCompra>();
        public static List<Resena> Resenas { get; set; } = new List<Resena>();
        public static List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public static List<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
        public static List<TransaccionPago> TransaccionesPago { get; set; } = new List<TransaccionPago>();
        public static List<BitacoraTransacciones> BitacoraTransacciones { get; set; } = new List<BitacoraTransacciones>();
        public static List<BitacoraErrores> BitacoraErrores { get; set; } = new List<BitacoraErrores>();

        static MockDatabase()
        {
            // Seed Admin User
            Usuarios.Add(new Usuario
            {
                UsuarioID = 1,
                Identificacion = "111111111",
                Nombre = "Admin",
                Apellido1 = "Scentify",
                Apellido2 = "Local",
                FechaNacimiento = new DateTime(1990, 1, 1),
                DocumentoIdentidad = "111111111",
                Rol = "Admin",
                Email = "Admin123@gmail.com",
                Contrasena = "Admin123",
                Activo = true
            });

            // Seed Client User
            Usuarios.Add(new Usuario
            {
                UsuarioID = 2,
                Identificacion = "222222222",
                Nombre = "Juan",
                Apellido1 = "Pérez",
                Apellido2 = "Rodríguez",
                FechaNacimiento = new DateTime(1995, 5, 15),
                DocumentoIdentidad = "222222222",
                Rol = "Cliente",
                Email = "cliente@scentify.com",
                Contrasena = "Cliente123",
                Activo = true
            });

            // Seed Categorias
            Categorias.Add(new Categoria { CategoriaID = 1, Nombre = "Perfumes" });
            Categorias.Add(new Categoria { CategoriaID = 2, Nombre = "Colonias" });
            Categorias.Add(new Categoria { CategoriaID = 3, Nombre = "Lociones" });
            Categorias.Add(new Categoria { CategoriaID = 4, Nombre = "Esencias" });

            // Seed Marcas
            Marcas.Add(new Marca { MarcaID = 1, Nombre = "Chanel" });
            Marcas.Add(new Marca { MarcaID = 2, Nombre = "Dior" });
            Marcas.Add(new Marca { MarcaID = 3, Nombre = "Hugo Boss" });
            Marcas.Add(new Marca { MarcaID = 4, Nombre = "Calvin Klein" });
            Marcas.Add(new Marca { MarcaID = 5, Nombre = "Versace" });

            // Seed Productos
            Productos.Add(new Producto
            {
                ProductoID = 1,
                Nombre = "Bleu de Chanel",
                Descripcion = "Un perfume aromático amaderado para hombres modernos y elegantes.",
                Precio = 120.00m,
                Stock = 15,
                CategoriaID = 1,
                MarcaID = 1,
                Genero = "Hombre",
                TamanioML = 100,
                Descuento = 10.00m,
                ImagenURL = "https://images.unsplash.com/photo-1541643600914-78b084683601?auto=format&fit=crop&w=400&h=400&q=80",
                MarcaNombre = "Chanel",
                CategoriaNombre = "Perfumes"
            });

            Productos.Add(new Producto
            {
                ProductoID = 2,
                Nombre = "Sauvage Dior",
                Descripcion = "Una composición rotundamente fresca, con notas nobles y salvajes de bergamota de Calabria.",
                Precio = 135.00m,
                Stock = 8,
                CategoriaID = 1,
                MarcaID = 2,
                Genero = "Hombre",
                TamanioML = 100,
                Descuento = 0.00m,
                ImagenURL = "https://images.unsplash.com/photo-1594035910387-fea47794261f?auto=format&fit=crop&w=400&h=400&q=80",
                MarcaNombre = "Dior",
                CategoriaNombre = "Perfumes"
            });

            Productos.Add(new Producto
            {
                ProductoID = 3,
                Nombre = "Boss Bottled",
                Descripcion = "El perfume del hombre de hoy: elegante, sofisticado y lleno de confianza.",
                Precio = 85.00m,
                Stock = 20,
                CategoriaID = 1,
                MarcaID = 3,
                Genero = "Hombre",
                TamanioML = 100,
                Descuento = 5.00m,
                ImagenURL = "https://images.unsplash.com/photo-1523293182086-7651a899d37f?auto=format&fit=crop&w=400&h=400&q=80",
                MarcaNombre = "Hugo Boss",
                CategoriaNombre = "Perfumes"
            });

            Productos.Add(new Producto
            {
                ProductoID = 4,
                Nombre = "CK One",
                Descripcion = "Fragancia unisex icónica que redefine las fronteras de la libertad individual.",
                Precio = 65.00m,
                Stock = 25,
                CategoriaID = 2,
                MarcaID = 4,
                Genero = "Unisex",
                TamanioML = 200,
                Descuento = 0.00m,
                ImagenURL = "https://images.unsplash.com/photo-1528740564264-7a912b73b0d5?auto=format&fit=crop&w=400&h=400&q=80",
                MarcaNombre = "Calvin Klein",
                CategoriaNombre = "Colonias"
            });

            Productos.Add(new Producto
            {
                ProductoID = 5,
                Nombre = "Eros Versace",
                Descripcion = "Fragancia de amor, pasión y sensualidad sublime.",
                Precio = 95.00m,
                Stock = 12,
                CategoriaID = 1,
                MarcaID = 5,
                Genero = "Hombre",
                TamanioML = 100,
                Descuento = 15.00m,
                ImagenURL = "https://images.unsplash.com/photo-1547887537-6158d64c35b3?auto=format&fit=crop&w=400&h=400&q=80",
                MarcaNombre = "Versace",
                CategoriaNombre = "Perfumes"
            });

            // Seed Resenas
            Resenas.Add(new Resena
            {
                ResenaID = 1,
                ProductoID = 1,
                UsuarioID = 2,
                Calificacion = 5,
                Comentario = "Excelente aroma, dura todo el día y tiene gran proyección.",
                Fecha = DateTime.Now.AddDays(-5),
                ProductoNombre = "Bleu de Chanel",
                UsuarioNombre = "Juan Pérez",
                Email = "cliente@scentify.com"
            });

            Resenas.Add(new Resena
            {
                ResenaID = 2,
                ProductoID = 2,
                UsuarioID = 2,
                Calificacion = 4,
                Comentario = "Un clásico, muy fresco pero bastante común hoy en día.",
                Fecha = DateTime.Now.AddDays(-3),
                ProductoNombre = "Sauvage Dior",
                UsuarioNombre = "Juan Pérez",
                Email = "cliente@scentify.com"
            });

            // Seed Pedidos
            Pedidos.Add(new Pedido
            {
                PedidoID = 1,
                UsuarioID = 2,
                DireccionEnvio = "San José, Costa Rica",
                TelefonoContacto = "+506 8888-8888",
                Total = 240.00m,
                FechaPedido = DateTime.Now.AddDays(-2),
                Estado = "Entregado",
                Notas = "Entregar después de las 5 PM",
                UsuarioNombre = "Juan Pérez",
                Email = "cliente@scentify.com"
            });

            // Seed DetallesPedido
            DetallesPedido.Add(new DetallePedido
            {
                DetallePedidoID = 1,
                PedidoID = 1,
                ProductoID = 1,
                Cantidad = 2,
                PrecioUnitario = 120.00m,
                Descuento = 10.00m,
                Subtotal = 240.00m,
                ProductoNombre = "Bleu de Chanel",
                ImagenURL = "https://images.unsplash.com/photo-1541643600914-78b084683601?auto=format&fit=crop&w=400&h=400&q=80"
            });

            // Seed TransaccionesPago
            TransaccionesPago.Add(new TransaccionPago
            {
                TransaccionPagoID = 1,
                PedidoID = 1,
                MetodoPago = "Tarjeta",
                Monto = 240.00m,
                Estado = "Aprobado",
                CodigoTransaccion = "TX_65893125",
                FechaCreacion = DateTime.Now.AddDays(-2)
            });

            // Seed BitacoraTransacciones
            BitacoraTransacciones.Add(new BitacoraTransacciones
            {
                BitacoraTransaccionesID = 1,
                Tabla = "Pedido",
                Accion = "INSERT",
                UsuarioID = 2,
                Fecha = DateTime.Now.AddDays(-2),
                Descripcion = "Se registró un nuevo pedido con ID 1."
            });
            BitacoraTransacciones.Add(new BitacoraTransacciones
            {
                BitacoraTransaccionesID = 2,
                Tabla = "Usuario",
                Accion = "UPDATE",
                UsuarioID = 1,
                Fecha = DateTime.Now.AddDays(-1),
                Descripcion = "Administrador actualizó configuración general del sistema."
            });
        }
    }
}
