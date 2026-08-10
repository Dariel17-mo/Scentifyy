using System;
using System.ComponentModel.DataAnnotations;

namespace Scentify.Models
{
    public class BitacoraErrores
    {
        public int BitacoraErroresID { get; set; }

        [Required(ErrorMessage = "El mensaje de error es obligatorio.")]
        [StringLength(4000, ErrorMessage = "El mensaje no puede tener más de 4000 caracteres.")]
        public string MensajeError { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
