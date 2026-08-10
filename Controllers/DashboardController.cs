using Microsoft.AspNetCore.Mvc;
using Scentify.Models;
using Microsoft.Data.SqlClient;

namespace Scentify.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string _connectionString;

        public DashboardController()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            var perfil = HttpContext.Session.GetString("Perfil");
            if (perfil != "Administrador")
                return RedirectToAction("Login", "Usuario");

            var dashboard = new DashboardViewModel();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Total ventas
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Total), 0) FROM Pedido", conn))
                    dashboard.TotalVentas = Convert.ToDecimal(cmd.ExecuteScalar());

                // Producto más vendido
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 1 P.Nombre 
                    FROM DetallePedido DP 
                    JOIN Producto P ON DP.ProductoID = P.ProductoID 
                    GROUP BY P.Nombre 
                    ORDER BY SUM(DP.Cantidad) DESC", conn))
                    dashboard.ProductoMasVendido = cmd.ExecuteScalar()?.ToString() ?? "N/A";

                // Total productos vendidos
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Cantidad), 0) FROM DetallePedido", conn))
                    dashboard.TotalProductosVendidos = Convert.ToInt32(cmd.ExecuteScalar());

                // Total pedidos
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pedido", conn))
                    dashboard.TotalPedidos = Convert.ToInt32(cmd.ExecuteScalar());

                // Ventas por mes
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT FORMAT(FechaPedido, 'yyyy-MM') AS Mes, 
                           SUM(Total) AS TotalMensual
                    FROM Pedido
                    GROUP BY FORMAT(FechaPedido, 'yyyy-MM')
                    ORDER BY Mes", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard.VentasMesLabels.Add(reader.GetString(0));
                            dashboard.VentasMesValores.Add(reader.GetDecimal(1));
                        }
                    }
                }

                // Top productos más vendidos
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 5 P.Nombre, SUM(DP.Cantidad), SUM(DP.Cantidad * DP.PrecioUnitario)
                    FROM DetallePedido DP
                    JOIN Producto P ON DP.ProductoID = P.ProductoID
                    GROUP BY P.Nombre
                    ORDER BY SUM(DP.Cantidad) DESC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard.TopProductos.Add(new ProductoTopViewModel
                            {
                                Nombre = reader.GetString(0),
                                CantidadVendida = reader.GetInt32(1),
                                TotalGenerado = reader.GetDecimal(2)
                            });
                        }
                    }
                }

                // Ventas por categoría (monto)
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT C.Nombre, SUM(DP.Cantidad * DP.PrecioUnitario)
                    FROM DetallePedido DP
                    JOIN Producto P ON DP.ProductoID = P.ProductoID
                    JOIN Categoria C ON P.CategoriaID = C.CategoriaID
                    GROUP BY C.Nombre", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard.CategoriasLabels.Add(reader.GetString(0));
                            dashboard.CategoriasVentas.Add(reader.GetDecimal(1));
                        }
                    }
                }

                // Top categorías por cantidad vendida
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 5 C.Nombre, SUM(DP.Cantidad)
                    FROM DetallePedido DP
                    JOIN Producto P ON DP.ProductoID = P.ProductoID
                    JOIN Categoria C ON P.CategoriaID = C.CategoriaID
                    GROUP BY C.Nombre
                    ORDER BY SUM(DP.Cantidad) DESC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard.CategoriasTopLabels.Add(reader.GetString(0));
                            dashboard.CategoriasTopCantidad.Add(reader.GetInt32(1));
                        }
                    }
                }

                // Usuarios que más han comprado (monto total)
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT U.Nombre, SUM(P.Total) AS TotalComprado
                    FROM Pedido P
                    JOIN Usuario U ON P.UsuarioID = U.UsuarioID
                    GROUP BY U.Nombre
                    ORDER BY TotalComprado DESC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard.UsuariosLabels.Add(reader.GetString(0));
                            dashboard.UsuariosMontos.Add(reader.GetDecimal(1));
                        }
                    }
                }

            }

            return View(dashboard);
        }
    }
}
