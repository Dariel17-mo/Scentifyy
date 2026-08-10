using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scentify.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioID { get; set; }

        [NotMapped]
        public string? UsuarioNombre { get; set; }

        [NotMapped]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La dirección de envío es obligatoria.")]
        [StringLength(255, ErrorMessage = "La dirección no puede tener más de 255 caracteres.")]
        public string DireccionEnvio { get; set; }

        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
        public string? TelefonoContacto { get; set; }

        [Required(ErrorMessage = "El total del pedido es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser un valor positivo.")]
        public decimal Total { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [StringLength(50, ErrorMessage = "El estado no puede tener más de 50 caracteres.")]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(500, ErrorMessage = "Las notas no pueden tener más de 500 caracteres.")]
        public string? Notas { get; set; }

        // Relación de navegación marcada como nullable para evitar validación y problemas de binding
        [ForeignKey("UsuarioID")]
        public Usuario? Usuario { get; set; }
    }
}
