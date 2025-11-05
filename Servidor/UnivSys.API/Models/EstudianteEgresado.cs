using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivSys.API.Models
{
    // 3C. Detalle para Estudiantes Egresados (Relación 1:1)
    public class EstudianteEgresado
    {
        [Key]
        [Column(TypeName = "VARCHAR(20)")]
        // IDEstudiante es la clave primaria y la clave foránea a Estudiantes
        public string IDEstudiante { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        // Se aplicará la restricción CHECK (<= GETDATE()) a nivel de base de datos
        public DateTime FechaEgreso { get; set; }

        // Propiedad de Navegación (para la relación 1:1)
        [ForeignKey("IDEstudiante")]
        public Estudiante Estudiante { get; set; }
    }
}