using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class Evento
{
    public int EventoId { get; set; }

    public string NombreEvento { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public string? Ubicacion { get; set; }

    public string? Descripcion { get; set; }

    public int? OrganizadorId { get; set; }

    public virtual Usuario? Organizador { get; set; }
    public string? ImagenUrl { get; set; }

    public ICollection<VoluntarioEvento> VoluntariosEventos { get; set; }
    public virtual ICollection<Voluntario> Voluntarios { get; set; } = new List<Voluntario>();


}
