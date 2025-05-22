using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class Tarea
{
    public int TareaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateOnly? FechaAsignacion { get; set; }

    public DateOnly? FechaLimite { get; set; }

    public string? Estado { get; set; }

    public int? VoluntarioId { get; set; }

    public int? AdministradorId { get; set; }

    public virtual Usuario? Administrador { get; set; }

    public virtual Voluntario? Voluntario { get; set; }
}
