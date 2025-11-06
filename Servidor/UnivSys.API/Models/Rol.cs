using System.ComponentModel.DataAnnotations;

namespace UnivSys.API.Models
{
    public class Rol
    {
        [Key]
        public int IDRol { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreRol { get; set; } // Ej: "Admin", "EmpleadoRegistro", "Consulta"
    }
}