using System.ComponentModel.DataAnnotations;

namespace UnivSys.API.Models.DTOs
{
    public class CalificacionDTO
    {
        public int IDRegistro { get; set; } // El ID de la calificación
        public string Materia { get; set; }
        public decimal Calificacion { get; set; }
        public string Periodo { get; set; }
    }
    public class EstudianteDetalleDTO
    {
        public string IDEstudiante { get; set; }
        public string Nombre { get; set; } 
        public string ApellidoPaterno { get; set; } 
        public string ApellidoMaterno { get; set; } 
        public int Semestre { get; set; }
        
        // Información de Carrera
        public int CarreraID { get; set; }
        public string NombreCarrera { get; set; } 

        // Información de Tipo de Estudiante
        public string TipoEstudiante { get; set; } // "Regular", "Becado", o "Egresado"
        
        // Detalle de Condición
        public int? PorcentajeBeca { get; set; } 
        public DateTime? FechaEgreso { get; set; }
        
        // Podríamos agregar el Promedio aquí en una fase posterior.
        public decimal? Promedio { get; set; }
        public List<CalificacionDTO> HistorialAcademico { get; set; } = new List<CalificacionDTO>();
    }
}