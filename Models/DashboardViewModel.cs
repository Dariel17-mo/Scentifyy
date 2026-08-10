namespace Scentify.Models
{
    public class DashboardViewModel
    {
        public decimal TotalVentas { get; set; }
        public string ProductoMasVendido { get; set; }
        public int TotalProductosVendidos { get; set; }
        public int TotalPedidos { get; set; }
        public List<string> VentasMesLabels { get; set; } = new();
        public List<decimal> VentasMesValores { get; set; } = new();
        public List<ProductoTopViewModel> TopProductos { get; set; } = new();
        public List<string> CategoriasLabels { get; set; } = new();
        public List<decimal> CategoriasVentas { get; set; } = new();
        public List<string> CategoriasTopLabels { get; set; } = new();
        public List<int> CategoriasTopCantidad { get; set; } = new();
        public List<string> MarcasLabels { get; set; } = new();
        public List<decimal> MarcasVentas { get; set; } = new();
        public List<string> UsuariosLabels { get; set; } = new();
        public List<decimal> UsuariosMontos { get; set; } = new();
    }

    public class ProductoTopViewModel
    {
        public string Nombre { get; set; }
        public int CantidadVendida { get; set; }
        public decimal TotalGenerado { get; set; }
    }

}
