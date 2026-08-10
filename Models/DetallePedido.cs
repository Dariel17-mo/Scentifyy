using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scentify.Models
{
    public class DetallePedido
    {
        public int DetallePedidoID { get; set; }

        [Required(ErrorMessage = "El ID del pedido es obligatorio.")]
        public int PedidoID { get; set; }

        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal PrecioUnitario { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo.")]
        public decimal Descuento { get; set; } = 0;

        [NotMapped]
        public string ProductoNombre { get; set; }

        [NotMapped]
        public string ImagenURL { get; set; }

        [NotMapped]
        public decimal Subtotal { get; set; }

        // Relaciones de navegación marcadas como nullable para evitar errores en binding y validación
        [ForeignKey("PedidoID")]
        public Pedido? Pedido { get; set; }

        [ForeignKey("ProductoID")]
        public Producto? Producto { get; set; }
    }
}
