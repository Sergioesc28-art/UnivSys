using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivSys.API.Models
{
    // 2. Tabla Principal de Estudiantes
    public class Estudiante
    {
        [Key]
        [Column(TypeName = "VARCHAR(20)")] // Especifica el tipo de SQL
        public string IDEstudiante { get; set; }

        [Required, MaxLength(100)]
        public string Nombre_s { get; set; }

        [Required, MaxLength(100)]
        public string ApellidoPaterno { get; set; }

        [Required, MaxLength(100)]
        public string ApellidoMaterno { get; set; }

        [Required]
        // Se aplicará la restricción CHECK (1-15) a nivel de base de datos
        public int Semestre { get; set; }

        // Clave Foránea a Carreras
        [Required]
        public int CarreraID { get; set; }

        // Propiedad de Navegación (Relación 1:N)
        [ForeignKey("CarreraID")]
        public Carrera Carrera { get; set; }

        // Banderas Booleanas para el Factory Method (BIT en SQL)
        public bool EsBecado { get; set; }
        public bool EsEgresado { get; set; }

        // Propiedades de Navegación 1:1 con las tablas de detalle
        public EstudianteBecado? DetalleBecado { get; set; }
        public EstudianteEgresado? DetalleEgresado { get; set; }

        // Propiedad de Navegación 1:N con Historial Académico
        public ICollection<HistorialAcademico> Historial { get; set; } = new List<HistorialAcademico>();
    }
}