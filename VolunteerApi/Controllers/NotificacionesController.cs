using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VolunteerApi.Services;

namespace VolunteerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly NotificacionService _notificacionService;

        public NotificacionesController(NotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificaciones()
        {
            try
            {
                var notificaciones = await _notificacionService.ObtenerTodasAsync();
                return Ok(notificaciones);
            }
            catch (Exception ex)
            {
                // Loguea ex.Message y ex.StackTrace aquí
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
