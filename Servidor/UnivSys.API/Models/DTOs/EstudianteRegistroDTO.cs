using System.ComponentModel.DataAnnotations;

namespace UnivSys.API.Models.DTOs
{
    public class EstudianteRegistroDTO
    {
        // Campos requeridos para Estudiantes
        [Required(ErrorMessage = "El ID del Estudiante es obligatorio.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El ID debe tener entre 5 y 20 caracteres.")]
        public string IDEstudiante { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre no debe contener números ni caracteres especiales.")]
        public string Nombre_s { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido paterno no debe contener números ni caracteres especiales.")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido materno no debe contener números ni caracteres especiales.")]
        public string ApellidoMaterno { get; set; }

        [Range(1, 15, ErrorMessage = "El semestre debe estar entre 1 y 15.")]
        public int Semestre { get; set; }

        [Required(ErrorMessage = "El ID de la Carrera es obligatorio.")]
        public int CarreraID { get; set; }

        // Banderas para el Factory Method
        public bool EsBecado { get; set; } = false;
        public bool EsEgresado { get; set; } = false;

        // Campos opcionales para Detalle (Becados)
        public int? PorcentajeBeca { get; set; } // Nullable

        // Campos opcionales para Detalle (Egresados)
        public DateTime? FechaEgreso { get; set; } // Nullable
    }
}