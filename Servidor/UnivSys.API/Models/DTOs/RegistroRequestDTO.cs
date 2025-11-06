using System.ComponentModel.DataAnnotations;
namespace UnivSys.API.Models.DTOs
{
    public class RegistroRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Se requiere un IDRol válido.")]
        public int IDRol { get; set; } // Ej: 1=Admin, 2=EmpleadoRegistro
    }
}