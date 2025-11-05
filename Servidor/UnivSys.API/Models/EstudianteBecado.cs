using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivSys.API.Models
{
    // 3B. Detalle para Estudiantes Becados (Relación 1:1)
    public class EstudianteBecado
    {
        [Key]
        [Column(TypeName = "VARCHAR(20)")]
        // IDEstudiante es la clave primaria y la clave foránea a Estudiantes
        public string IDEstudiante { get; set; }

        [Required]
        // Se aplicará la restricción CHECK (1-100) a nivel de base de datos
        public int PorcentajeBeca { get; set; }

        // Propiedad de Navegación (para la relación 1:1)
        [ForeignKey("IDEstudiante")]
        public Estudiante Estudiante { get; set; }
    }
}