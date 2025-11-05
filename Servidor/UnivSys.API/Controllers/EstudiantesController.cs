using Microsoft.AspNetCore.Mvc;
using UnivSys.API.Models.DTOs;
using UnivSys.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace UnivSys.API.Controllers
{
    // RUTA BASE: /api/estudiantes
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly EstudianteService _estudianteService;

        // Inyección del servicio
        public EstudiantesController(EstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

     
        public async Task<IActionResult> RegistrarEstudiante([FromBody] EstudianteRegistroDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request por DataAnnotations
            }

            var (estudianteDetalle, errores) = await _estudianteService.RegistrarEstudianteAsync(dto);

            if (errores.Any())
            {
                // 400 Bad Request por Errores de Negocio (Duplicidad, Factory Method)
                return BadRequest(new { ErroresDeNegocio = errores });
            }

            // 201 Created: Devuelve la ubicación del nuevo recurso
            return CreatedAtAction(nameof(RegistrarEstudiante), new { id = estudianteDetalle!.IDEstudiante }, estudianteDetalle);
        }

        
        [HttpGet]
        // [Authorize] 
        public async Task<IActionResult> GetEstudiantes(
            [FromQuery] int? carreraId, 
            [FromQuery] int? semestre)
        {
            var errores = new List<string>();
            
            // Validación de filtros: Semestre
            if (semestre.HasValue && (semestre.Value < 1 || semestre.Value > 15))
            {
                errores.Add("El valor de 'semestre' debe estar entre 1 y 15.");
            }
            
            if (errores.Any())
            {
                return BadRequest(new { ErroresDeFiltro = errores }); // 400 Bad Request
            }

            var listaEstudiantes = await _estudianteService.GetEstudiantesFiltradosAsync(carreraId, semestre);

            // 200 OK: Devuelve la lista, aunque esté vacía
            return Ok(listaEstudiantes);
        }


        [HttpGet("{idEstudiante}")]
        // [Authorize] 
        public async Task<IActionResult> GetEstudiante(string idEstudiante)
        {
            var estudianteDetalle = await _estudianteService.GetEstudianteByIdAsync(idEstudiante);

            if (estudianteDetalle == null)
            {
                return NotFound(new { Mensaje = $"Estudiante con ID '{idEstudiante}' no encontrado." }); // 404 Not Found
            }

            return Ok(estudianteDetalle); // 200 OK
        }

    
        [HttpPut("{idEstudiante}")]
        // [Authorize] 
        public async Task<IActionResult> ActualizarEstudiante(string idEstudiante, [FromBody] EstudianteRegistroDTO dto)
        {
            // Validar que el ID del DTO coincida con el ID de la ruta
            if (idEstudiante != dto.IDEstudiante)
            {
                return BadRequest(new { Error = "El ID del estudiante en la ruta no coincide con el ID en el cuerpo de la petición." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // Este método debe ser implementado en el servicio a continuación
            var (estudianteDetalle, errores, encontrado) = await _estudianteService.ActualizarEstudianteAsync(idEstudiante, dto);

            if (!encontrado)
            {
                return NotFound(new { Mensaje = $"Estudiante con ID '{idEstudiante}' no encontrado para actualizar." }); // 404 Not Found
            }
            
            if (errores.Any())
            {
                return BadRequest(new { ErroresDeNegocio = errores }); // 400 Bad Request
            }
            
            return Ok(estudianteDetalle); // 200 OK
        }
        
      
        [HttpDelete("{idEstudiante}")]
        // [Authorize] 
        public async Task<IActionResult> EliminarEstudiante(string idEstudiante)
        {
            // Este método debe ser implementado en el servicio a continuación
            var eliminado = await _estudianteService.EliminarEstudianteAsync(idEstudiante);

            if (!eliminado)
            {
                return NotFound(new { Mensaje = $"Estudiante con ID '{idEstudiante}' no encontrado para eliminar." }); // 404 Not Found
            }
            
            return NoContent(); // 204 No Content (Éxito sin contenido de respuesta)
        }
    }
}