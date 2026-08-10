using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scentify.Models
{
    public class CarritoCompra
    {
        public int CarritoCompraID { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Cantidad { get; set; }

        [NotMapped]
        public string ProductoNombre { get; set; }

        [NotMapped]
        public decimal Precio { get; set; }

        [NotMapped]
        public decimal Descuento { get; set; }

        [NotMapped]
        public decimal Subtotal { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaAgregado { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }

        [NotMapped]
        public string ImagenURL { get; set; }


        // Relaciones de navegación nullable para evitar errores en binding/validación
        [ForeignKey("UsuarioID")]
        public Usuario? Usuario { get; set; }

        [ForeignKey("ProductoID")]
        public Producto? Producto { get; set; }
    }
}
