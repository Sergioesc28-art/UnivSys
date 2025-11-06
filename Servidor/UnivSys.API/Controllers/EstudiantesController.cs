using Microsoft.AspNetCore.Mvc;
using UnivSys.API.Models.DTOs;
using UnivSys.API.Services;
using Microsoft.AspNetCore.Authorization; // ¡Importante!

namespace UnivSys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //
    // ¡AQUÍ ESTÁ LA SEGURIDAD!
    // Protege toda la clase. Solo permite el acceso si el token JWT
    // contiene UNO de estos tres roles: "Maestro", "Director" O "Admin".
    //
    [Authorize(Roles = "Maestro,Director,Admin")]
    public class EstudiantesController : ControllerBase
    {
        private readonly EstudianteService _estudianteService;

        public EstudiantesController(EstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        // =======================================================================
        // 1. POST: Registrar Estudiante
        // (Hereda la autorización de la clase)
        // =======================================================================
        [HttpPost] 
        // Si quisieras ser MÁS específico, podrías añadir:
        // [Authorize(Roles = "Admin,Director")]
        public async Task<IActionResult> RegistrarEstudiante([FromBody] EstudianteRegistroDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var (estudianteDetalle, errores) = await _estudianteService.RegistrarEstudianteAsync(dto);

            if (errores.Any())
            {
                return BadRequest(new { ErroresDeNegocio = errores });
            }

            return CreatedAtAction(nameof(RegistrarEstudiante), new { id = estudianteDetalle!.IDEstudiante }, estudianteDetalle);
        }

        // =======================================================================
        // 2A. GET: Consulta Masiva y Filtrado
        // (Hereda la autorización de la clase)
        // =======================================================================
        [HttpGet]
        public async Task<IActionResult> GetEstudiantes(
            [FromQuery] int? carreraId, 
            [FromQuery] int? semestre)
        {
            var errores = new List<string>();
            
            if (semestre.HasValue && (semestre.Value < 1 || semestre.Value > 15))
            {
                errores.Add("El valor de 'semestre' debe estar entre 1 y 15.");
            }
            
            if (errores.Any())
            {
                return BadRequest(new { ErroresDeFiltro = errores }); 
            }

            var listaEstudiantes = await _estudianteService.GetEstudiantesFiltradosAsync(carreraId, semestre);
            return Ok(listaEstudiantes);
        }

        // =======================================================================
        // 2B. GET: Consulta Individual por ID
        // (Hereda la autorización de la clase)
        // =======================================================================
        [HttpGet("{idEstudiante}")]
        public async Task<IActionResult> GetEstudiante(string idEstudiante)
        {
            var estudianteDetalle = await _estudianteService.GetEstudianteByIdAsync(idEstudiante);

            if (estudianteDetalle == null)
            {
                return NotFound(new { Mensaje = $"Estudiante con ID '{idEstudiante}' no encontrado." }); 
            }

            return Ok(estudianteDetalle); 
        }

        // =======================================================================
        // 3. PUT: Modificar Estudiante
        // (Hereda la autorización de la clase)
        // =======================================================================
        [HttpPut("{idEstudiante}")]
        public async Task<IActionResult> ActualizarEstudiante(string idEstudiante, [FromBody] EstudianteRegistroDTO dto)
        {
            if (idEstudiante != dto.IDEstudiante)
            {
                return BadRequest(new { Error = "El ID del estudiante en la ruta no coincide con el ID en el cuerpo de la petición." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var (estudianteDetalle, errores, encontrado) = await _estudianteService.ActualizarEstudianteAsync(idEstudiante, dto);

            if (!encontrado)
            {
                return NotFound(new { Mensaje = $"Estudiante con ID '{idEstudiante}' no encontrado para actualizar." }); 
            }
            
            if (errores.Any())
            {
                return BadRequest(new { ErroresDeNegocio = errores }); 
            }
            
            return Ok(estudianteDetalle); 
        }
        
        // =======================================================================
        // 4. DELETE: Eliminar Estudiante
        // (Hereda la autorización de la clase, pero la RESTRINGIMOS MÁS)
        // =======================================================================
        [HttpDelete("{idEstudiante}")]
        [Authorize(Roles = "Director,Admin")] // Solo Director o Admin pueden borrar
        public async Task<IActionResult> EliminarEstudiante(string idEstudiante)
        {
            var eliminado = await _estudianteService.EliminarEstudianteAsync(idEstudiante);

            if (!eliminado)
            {
                return NotFound(new { Mensaje = $"Estudiante con ID '{idEstudiante}' no encontrado para eliminar." }); 
            }
            
            return NoContent(); 
        }
    }
}