using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VolunteerApi.Models
{
    public class NotificacionEvento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }
    }
}
