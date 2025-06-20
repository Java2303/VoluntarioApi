using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace VolunteerApi.Models
{
    public class NotificacionEvento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("titulo")]
        public string Titulo { get; set; } = "Nuevo Evento";

        [BsonElement("mensaje")]
        public string Mensaje { get; set; }

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [BsonElement("leido")]
        public bool Leido { get; set; } = false;
    }
}
