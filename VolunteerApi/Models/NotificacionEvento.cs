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

        [BsonElement("mensaje")]
        public string Mensaje { get; set; }

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("eventoId")]
        public int EventoId { get; set; }

        [BsonElement("leido")]
        public bool Leido { get; set; } = false;
    }
}
