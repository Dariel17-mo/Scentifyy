using System;
using System.ComponentModel.DataAnnotations;

namespace Scentify.Models
{
    public class BitacoraTransacciones
    {
        public int BitacoraTransaccionesID { get; set; }

        [Required(ErrorMessage = "El nombre de la tabla es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre de la tabla no puede tener más de 100 caracteres.")]
        public string Tabla { get; set; }

        [Required(ErrorMessage = "La acción es obligatoria.")]
        [StringLength(10, ErrorMessage = "La acción no puede tener más de 10 caracteres.")]
        public string Accion { get; set; }

        public int? UsuarioID { get; set; } // Puede ser nulo si la acción fue del sistema

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public string Descripcion { get; set; } // Sin límite de longitud, tipo NVARCHAR(MAX)

        public string? UsuarioNombre { get; set; } // propiedad auxiliar para mostrar el nombre

    }
}
