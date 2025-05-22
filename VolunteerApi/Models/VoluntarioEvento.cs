using VolunteerApi.Models;
public class VoluntarioEvento
{
    public int VoluntarioEventoId { get; set; }
    public int VoluntarioId { get; set; }
    public int EventoId { get; set; }
    public DateTime FechaAsignacion { get; set; }

    public Voluntario Voluntario { get; set; }
    public Evento Evento { get; set; }
}

