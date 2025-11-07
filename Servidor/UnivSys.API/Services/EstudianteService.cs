using UnivSys.API.Core.Factories;
using UnivSys.API.Data;
using UnivSys.API.Models;
using UnivSys.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace UnivSys.API.Services
{
    // Clase Central de Lógica de Negocio (Servidor Lógico)
    public class EstudianteService
    {
        private readonly AppDbContext _context;

        public EstudianteService(AppDbContext context)
        {
            _context = context;
        }

        // ===================================================================
        // HELPERS (Mapeo)
        // ===================================================================
        private EstudianteDetalleDTO MapearADetalleDTO(Estudiante estudiante, string nombreCarrera)
        {
            var tipo = "Regular";
            if (estudiante.EsBecado) tipo = "Becado";
            else if (estudiante.EsEgresado) tipo = "Egresado";

            return new EstudianteDetalleDTO
            {
                IDEstudiante = estudiante.IDEstudiante,
                Nombre = estudiante.Nombre_s,
                ApellidoPaterno = estudiante.ApellidoPaterno,
                ApellidoMaterno = estudiante.ApellidoMaterno,
                Semestre = estudiante.Semestre,
                CarreraID = estudiante.CarreraID,
                NombreCarrera = nombreCarrera,
                TipoEstudiante = tipo,
                PorcentajeBeca = estudiante.DetalleBecado?.PorcentajeBeca,
                FechaEgreso = estudiante.DetalleEgresado?.FechaEgreso
            };
        }

        // ===================================================================
        // 1. CREATE (POST) - Lógica de Registro con Factory Method
        // ===================================================================
        public async Task<(EstudianteDetalleDTO? dto, List<string> errores)> RegistrarEstudianteAsync(EstudianteRegistroDTO registroDto)
        {
            var errores = new List<string>();
            
            // 1. Validaciones Preliminares (Exclusividad y Duplicidad)
            if (registroDto.EsBecado && registroDto.EsEgresado)
            {
                errores.Add("Validación Exclusiva: Un estudiante no puede ser Becado y Egresado al mismo tiempo.");
            }
            if (await _context.Estudiantes.AnyAsync(e => e.IDEstudiante == registroDto.IDEstudiante))
            {
                errores.Add($"El ID de estudiante '{registroDto.IDEstudiante}' ya existe en la base de datos.");
            }
            if (errores.Any())
            {
                return (null, errores);
            }

            // 2. Mapeo del DTO al Modelo
            var nuevoEstudiante = new Estudiante
            {
                IDEstudiante = registroDto.IDEstudiante,
                Nombre_s = registroDto.Nombre_s,
                ApellidoPaterno = registroDto.ApellidoPaterno,
                ApellidoMaterno = registroDto.ApellidoMaterno,
                Semestre = registroDto.Semestre,
                CarreraID = registroDto.CarreraID,
                EsBecado = registroDto.EsBecado,
                EsEgresado = registroDto.EsEgresado,
            };

            // 3. Mapeo de Tablas de Detalle y Contexto de Factory
            if (registroDto.EsBecado && registroDto.PorcentajeBeca.HasValue)
            {
                // **CORRECCIÓN:** Uso explícito de 'Models.' para evitar ambigüedad
                nuevoEstudiante.DetalleBecado = new Models.EstudianteBecado 
                { 
                    IDEstudiante = nuevoEstudiante.IDEstudiante, 
                    PorcentajeBeca = registroDto.PorcentajeBeca.Value 
                };
            }
            if (registroDto.EsEgresado && registroDto.FechaEgreso.HasValue)
            {
                // **CORRECCIÓN:** Uso explícito de 'Models.' para evitar ambigüedad
                nuevoEstudiante.DetalleEgresado = new Models.EstudianteEgresado 
                { 
                    IDEstudiante = nuevoEstudiante.IDEstudiante, 
                    FechaEgreso = registroDto.FechaEgreso.Value 
                };
            }
            
            var usuarioTipo = UsuarioFactory.CrearInstancia(nuevoEstudiante);

            // 4. Validaciones Específicas del Patrón Factory
            var erroresEspecificos = usuarioTipo.ValidarDatosEspecificos();
            if (erroresEspecificos.Any())
            {
                return (null, erroresEspecificos);
            }
            
            // 5. Persistencia y Lógica de Aplicación
            _context.Estudiantes.Add(nuevoEstudiante);
            await _context.SaveChangesAsync();
            usuarioTipo.AplicarLogica(); 

            // 6. Mapeo Final de Respuesta
            var carrera = await _context.Carreras.FindAsync(nuevoEstudiante.CarreraID);
            return (MapearADetalleDTO(nuevoEstudiante, carrera?.NombreCarrera ?? "Desconocida"), new List<string>());
        }

        // ===================================================================
        // 2A. READ (GET) - Consulta Individual
        // ===================================================================
        public async Task<EstudianteDetalleDTO?> GetEstudianteByIdAsync(string idEstudiante)
        {
            // Incluimos las relaciones para mapear el DTO correctamente
            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .Include(e => e.DetalleBecado)
                .Include(e => e.DetalleEgresado)
                .FirstOrDefaultAsync(e => e.IDEstudiante == idEstudiante);

            if (estudiante == null)
            {
                return null;
            }

            return MapearADetalleDTO(estudiante, estudiante.Carrera?.NombreCarrera ?? "Desconocida");
        }

        // ===================================================================
        // 2B. READ (GET) - Consulta Masiva y Filtrado
        // ===================================================================
        public async Task<List<EstudianteDetalleDTO>> GetEstudiantesFiltradosAsync(int? carreraId, int? semestre)
        {
            var query = _context.Estudiantes
                .Include(e => e.Carrera)
                .Include(e => e.DetalleBecado)
                .Include(e => e.DetalleEgresado)
                .AsQueryable();

            if (carreraId.HasValue)
            {
                query = query.Where(e => e.CarreraID == carreraId.Value);
            }

            if (semestre.HasValue)
            {
                query = query.Where(e => e.Semestre == semestre.Value);
            }
            
            var estudiantes = await query.ToListAsync();

            var listaDetalleDTO = estudiantes.Select(e => 
                MapearADetalleDTO(e, e.Carrera?.NombreCarrera ?? "Desconocida"))
                .ToList();

            return listaDetalleDTO;
        }


        // ===================================================================
        // 3. UPDATE (PUT) - Modificación con lógica Factory
        // ===================================================================
        // El resultado de la tupla ahora tiene 3 elementos: DTO, Errores, y un booleano para indicar si fue Encontrado.
        public async Task<(EstudianteDetalleDTO? dto, List<string> errores, bool encontrado)> ActualizarEstudianteAsync(string idEstudiante, EstudianteRegistroDTO registroDto)
        {
            // Carga el estudiante, incluyendo sus detalles relacionados
            var estudianteExistente = await _context.Estudiantes
                .Include(e => e.DetalleBecado)
                .Include(e => e.DetalleEgresado)
                .FirstOrDefaultAsync(e => e.IDEstudiante == idEstudiante);

            if (estudianteExistente == null)
            {
                return (null, new List<string>(), false); // No encontrado
            }

            var errores = new List<string>();
            
            // 1. Validación de Exclusividad
            if (registroDto.EsBecado && registroDto.EsEgresado)
            {
                errores.Add("Validación Exclusiva: Un estudiante no puede ser Becado y Egresado al mismo tiempo.");
                return (null, errores, true);
            }
            
            // 2. Aplicar Actualizaciones Simples
            estudianteExistente.Nombre_s = registroDto.Nombre_s;
            estudianteExistente.ApellidoPaterno = registroDto.ApellidoPaterno;
            estudianteExistente.ApellidoMaterno = registroDto.ApellidoMaterno;
            estudianteExistente.Semestre = registroDto.Semestre;
            estudianteExistente.CarreraID = registroDto.CarreraID;
            
            // 3. Lógica de Actualización de Tablas de Detalle (Factory State Change)
            
            // 3.1. TRANSICIÓN A BECADO
            if (registroDto.EsBecado && estudianteExistente.DetalleBecado == null)
            {
                estudianteExistente.DetalleBecado = new Models.EstudianteBecado { IDEstudiante = idEstudiante, PorcentajeBeca = registroDto.PorcentajeBeca ?? 0 };
                _context.EstudiantesEgresados.Remove(estudianteExistente.DetalleEgresado!); 
                estudianteExistente.DetalleEgresado = null;
                estudianteExistente.EsBecado = true;
                estudianteExistente.EsEgresado = false;
            } 
            // 3.2. TRANSICIÓN A EGRESADO
            else if (registroDto.EsEgresado && estudianteExistente.DetalleEgresado == null)
            {
                estudianteExistente.DetalleEgresado = new Models.EstudianteEgresado { IDEstudiante = idEstudiante, FechaEgreso = registroDto.FechaEgreso ?? DateTime.Today };
                _context.EstudiantesBecados.Remove(estudianteExistente.DetalleBecado!);
                estudianteExistente.DetalleBecado = null;
                estudianteExistente.EsEgresado = true;
                estudianteExistente.EsBecado = false;
            }
            // 3.3. TRANSICIÓN A REGULAR
            else if (!registroDto.EsBecado && !registroDto.EsEgresado) 
            {
                if (estudianteExistente.DetalleBecado != null) _context.EstudiantesBecados.Remove(estudianteExistente.DetalleBecado);
                if (estudianteExistente.DetalleEgresado != null) _context.EstudiantesEgresados.Remove(estudianteExistente.DetalleEgresado);
                estudianteExistente.DetalleBecado = null;
                estudianteExistente.DetalleEgresado = null;
                estudianteExistente.EsBecado = false;
                estudianteExistente.EsEgresado = false;
            }
            // 3.4. Actualización de valores existentes
            else if (estudianteExistente.DetalleBecado != null && registroDto.PorcentajeBeca.HasValue)
            {
                estudianteExistente.DetalleBecado.PorcentajeBeca = registroDto.PorcentajeBeca.Value;
            }
            else if (estudianteExistente.DetalleEgresado != null && registroDto.FechaEgreso.HasValue)
            {
                estudianteExistente.DetalleEgresado.FechaEgreso = registroDto.FechaEgreso.Value;
            }
            
            // 4. Re-ejecutar validaciones específicas del Patrón Factory
            var usuarioTipo = UsuarioFactory.CrearInstancia(estudianteExistente);
            var erroresEspecificos = usuarioTipo.ValidarDatosEspecificos();
            
            if (erroresEspecificos.Any())
            {
                return (null, erroresEspecificos, true);
            }

            // 5. Persistencia y Mapeo
            await _context.SaveChangesAsync();
            var carrera = await _context.Carreras.FindAsync(estudianteExistente.CarreraID);
            
            return (MapearADetalleDTO(estudianteExistente, carrera?.NombreCarrera ?? "Desconocida"), new List<string>(), true);
        }

        // ===================================================================
        // 4. DELETE (DELETE) - Baja con Transacción
        // ===================================================================
        public async Task<bool> EliminarEstudianteAsync(string idEstudiante)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.DetalleBecado)
                .Include(e => e.DetalleEgresado)
                .Include(e => e.Historial)
                .FirstOrDefaultAsync(e => e.IDEstudiante == idEstudiante);

            if (estudiante == null)
            {
                return false; 
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Eliminar dependencias
                    if (estudiante.DetalleBecado != null) _context.EstudiantesBecados.Remove(estudiante.DetalleBecado);
                    if (estudiante.DetalleEgresado != null) _context.EstudiantesEgresados.Remove(estudiante.DetalleEgresado);
                    _context.HistorialAcademico.RemoveRange(estudiante.Historial);
                    
                    // Eliminar registro principal
                    _context.Estudiantes.Remove(estudiante);
                    
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}