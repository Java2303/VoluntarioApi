using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class Curso
{
    public int CursoId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string? Categoria { get; set; }

    public string? Dificultad { get; set; }

    public virtual ICollection<HistorialCapacitacion> HistorialCapacitacions { get; set; } = new List<HistorialCapacitacion>();
}
