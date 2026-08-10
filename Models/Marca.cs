using System.ComponentModel.DataAnnotations;

namespace Scentify.Models
{
    public class Marca
    {
        public int MarcaID { get; set; }

        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }
    }
}
