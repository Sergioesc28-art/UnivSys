using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivSys.API.Models
{
    public class Usuario
    {
        [Key]
        public int IDUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } // El "login"

        [Required]
        public string PasswordHash { get; set; } // NUNCA guardes contraseñas en texto plano

        // Clave Foránea a Roles
        public int IDRol { get; set; }

        [ForeignKey("IDRol")]
        public Rol Rol { get; set; }
    }
}