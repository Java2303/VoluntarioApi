using MongoDB.Driver;
using VolunteerApi.Services;
using VolunteerApi.Models;


namespace VolunteerApi.Services
{
    public class NotificacionService
    {
        private readonly IMongoCollection<NotificacionEvento> _notificaciones;

        public NotificacionService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
            _notificaciones = database.GetCollection<NotificacionEvento>("Notificaciones");
        }

        public async Task CrearNotificacionAsync(string mensaje)
        {
            var notificacion = new NotificacionEvento
            {
                Mensaje = mensaje,
                Fecha = DateTime.UtcNow
            };

            await _notificaciones.InsertOneAsync(notificacion);
        }

        public async Task<List<NotificacionEvento>> ObtenerTodasAsync()
        {
            return await _notificaciones.Find(_ => true).ToListAsync();
        }
    }
}
