using System;
using System.ComponentModel.DataAnnotations;

namespace Scentify.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(50, ErrorMessage = "La identificación no puede tener más de 50 caracteres.")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El primer apellido no puede tener más de 100 caracteres.")]
        public string Apellido1 { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El segundo apellido no puede tener más de 100 caracteres.")]
        public string Apellido2 { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El documento de identidad es obligatorio.")]
        [StringLength(50, ErrorMessage = "El documento de identidad no puede tener más de 50 caracteres.")]
        public string DocumentoIdentidad { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(50, ErrorMessage = "El rol no puede tener más de 50 caracteres.")]
        public string Rol { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        [StringLength(100, ErrorMessage = "El email no puede tener más de 100 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, ErrorMessage = "La contraseña no puede tener más de 255 caracteres.")]
        public string Contrasena { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UltimoLogin { get; set; }

        public bool Activo { get; set; } = true;
    }
}
