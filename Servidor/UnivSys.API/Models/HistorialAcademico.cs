using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivSys.API.Models
{
    // 3A. Historial Académico
    public class HistorialAcademico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDRegistro { get; set; }

        // Clave Foránea a Estudiantes
        [Required]
        [Column(TypeName = "VARCHAR(20)")]
        public string IDEstudiante { get; set; }

        [Required, MaxLength(100)]
        public string Materia { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(4, 2)")]
        // Se aplicará la restricción CHECK (0.0 - 100.0) a nivel de base de datos
        public decimal Calificacion { get; set; }

        [Required, MaxLength(50)]
        public string Periodo { get; set; }

        // Propiedad de Navegación (Relación N:1)
        [ForeignKey("IDEstudiante")]
        public Estudiante Estudiante { get; set; }
    }
}