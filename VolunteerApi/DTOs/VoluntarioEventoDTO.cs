namespace VolunteerApi.DTOs
{
    public class VoluntarioEventoDTO
    {
        public int VoluntarioEventoId { get; set; }
        public int VoluntarioId { get; set; }
        public int EventoId { get; set; }
        public DateTime FechaAsignacion { get; set; }

        public string NombreVoluntario { get; set; }
        public string NombreEvento { get; set; }
    }

}
