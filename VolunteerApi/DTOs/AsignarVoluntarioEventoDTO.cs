namespace VolunteerApi.DTOs
{
    public class AsignarVoluntarioEventoDto
    {
        public int VoluntarioId { get; set; }
        public int EventoId { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }

}
