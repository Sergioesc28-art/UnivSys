using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivSys.API.Models
{
    // A. Tabla de Carreras
    public class Carrera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDCarrera { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreCarrera { get; set; }
        
        // Propiedad de navegación para EF Core: permite acceder a la lista de estudiantes de esta carrera
        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
    }
}