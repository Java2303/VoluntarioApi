using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VolunteerApi.Models;

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
            var notificaciones = await _notificacionService.ObtenerTodasAsync();
            return Ok(notificaciones);
        }
    }
}
