using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolunteerApi.DTOs;
using VolunteerApi.Models;

namespace VolunteerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoluntariosEventosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VoluntariosEventosController> _logger;

        public VoluntariosEventosController(AppDbContext context, ILogger<VoluntariosEventosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<ActionResult<IEnumerable<VoluntarioEventoDTO>>> GetVoluntariosEvento(int eventoId)
        {
            try
            {
                // Verificar si eventoId es válido
                if (eventoId <= 0)
                {
                    return BadRequest("ID de evento no válido.");
                }

                // Obtener las asignaciones de voluntarios a un evento específico
                var asignaciones = await _context.VoluntarioEventos
                    .Where(ve => ve.EventoId == eventoId)
                    .Include(ve => ve.Voluntario)  // Asegúrate de que la relación con Voluntario se carga correctamente
                    .Include(ve => ve.Evento)      // Asegúrate de que la relación con Evento también se carga
                    .Select(ve => new VoluntarioEventoDTO
                    {
                        VoluntarioEventoId = ve.VoluntarioEventoId,
                        VoluntarioId = ve.VoluntarioId,
                        EventoId = ve.EventoId,
                        FechaAsignacion = ve.FechaAsignacion,
                        NombreVoluntario = ve.Voluntario != null ? ve.Voluntario.Usuario.Nombre : "Sin nombre", // Asegúrate de que el Nombre sea accesible
                        NombreEvento = ve.Evento != null ? ve.Evento.NombreEvento : "Sin nombre de evento" // Asegúrate de que NombreEvento sea accesible
                    })
                    .ToListAsync();

                // Si no se encuentran asignaciones
                if (asignaciones == null || !asignaciones.Any())
                {
                    return NotFound(new { message = "No se encontraron voluntarios asignados a este evento." });
                }

                return Ok(asignaciones);
            }
            catch (Exception ex)
            {
                // Manejo de errores más detallado
                _logger.LogError(ex, "Error al obtener asignaciones del evento");
                return StatusCode(500, new { message = "Ocurrió un error al obtener las asignaciones.", details = ex.Message });
            }
        }


        // POST: Asignar un voluntario a un evento
        [HttpPost("asignar")]

        public async Task<IActionResult> AsignarVoluntario([FromBody] AsignarVoluntarioEventoDto dto)
        {
            if (dto == null || dto.VoluntarioId <= 0 || dto.EventoId <= 0)
            {
                return BadRequest("Datos incompletos o inválidos para la asignación.");
            }

            try
            {
                var asignacion = new VoluntarioEvento
                {
                    VoluntarioId = dto.VoluntarioId,
                    EventoId = dto.EventoId,
                    FechaAsignacion = dto.FechaAsignacion
                };

                _context.VoluntarioEventos.Add(asignacion);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Voluntario asignado correctamente al evento." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar voluntario a evento: {Message}", ex.Message);
                return StatusCode(500, new { message = "Ocurrió un error al asignar el voluntario.", details = ex.Message });
            }
        }

        // DELETE: Eliminar todas las asignaciones para un evento
        [HttpDelete("evento/{eventoId}")]
        public async Task<IActionResult> EliminarAsignacionesPorEvento(int eventoId)
        {
            try
            {
                var asignaciones = _context.VoluntarioEventos.Where(ve => ve.EventoId == eventoId);

                if (!asignaciones.Any())
                {
                    return NotFound(new { message = "No hay asignaciones para este evento." });
                }

                _context.VoluntarioEventos.RemoveRange(asignaciones);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar asignaciones del evento {EventoId}: {Message}", eventoId, ex.Message);
                return StatusCode(500, new { message = "Ocurrió un error al eliminar las asignaciones.", details = ex.Message });
            }
        }
    }
}
