using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class HistorialCapacitacion
{
    public int HistorialId { get; set; }

    public int? VoluntarioId { get; set; }

    public int? CursoId { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public string? Estado { get; set; }

    public virtual Curso? Curso { get; set; }

    public virtual Voluntario? Voluntario { get; set; }
}
