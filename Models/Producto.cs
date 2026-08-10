using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scentify.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int CategoriaID { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public int MarcaID { get; set; }

        [StringLength(20, ErrorMessage = "El género no puede tener más de 20 caracteres.")]
        public string? Genero { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El tamaño debe ser un número positivo.")]
        public int? TamanioML { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo.")]
        public decimal Descuento { get; set; } = 0;

        [StringLength(255, ErrorMessage = "La URL de la imagen no puede tener más de 255 caracteres.")]
        public string? ImagenURL { get; set; }

        // Relaciones de navegación
        [ForeignKey("CategoriaID")]
        public Categoria? Categoria { get; set; }

        [NotMapped]
        public string? CategoriaNombre { get; set; }

        [ForeignKey("MarcaID")]
        public Marca? Marca { get; set; }

        [NotMapped]
        public string? MarcaNombre { get; set; }


        public List<Resena> Resenas { get; set; } = new List<Resena>();
    }
}

