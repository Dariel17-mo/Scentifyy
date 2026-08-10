using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scentify.Models
{
    public class Resena
    {
        public int ResenaID { get; set; }

        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioID { get; set; }

        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int? Calificacion { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede tener más de 500 caracteres.")]
        public string Comentario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        // Relaciones de navegación como nullable para evitar validaciones indeseadas en post
        [ForeignKey("ProductoID")]
        public Producto? Producto { get; set; }

        [NotMapped]
        public string ProductoNombre { get; set; }

        [ForeignKey("UsuarioID")]
        public Usuario? Usuario { get; set; }

        [NotMapped]
        public string UsuarioNombre { get; set; }

        [NotMapped]
        public string Email { get; set; }
    }
}
