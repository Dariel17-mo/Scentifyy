using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scentify.Models
{
    public class TransaccionPago
    {
        public int TransaccionPagoID { get; set; }

        [Required(ErrorMessage = "El ID del pedido es obligatorio.")]
        public int PedidoID { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [StringLength(50, ErrorMessage = "El método de pago no puede tener más de 50 caracteres.")]
        public string MetodoPago { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(50, ErrorMessage = "El estado no puede tener más de 50 caracteres.")]
        public string Estado { get; set; }

        [StringLength(100, ErrorMessage = "El código de transacción no puede tener más de 100 caracteres.")]
        public string CodigoTransaccion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Propiedad de navegación nullable para evitar validaciones que exijan su carga
        [ForeignKey("PedidoID")]
        public Pedido? Pedido { get; set; }

        // Propiedad auxiliar para mostrar nombre o código del pedido, sin mapear ni validar
        [NotMapped]
        public string PedidoDescripcion { get; set; }
    }
}
