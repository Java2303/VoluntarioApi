using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class Especialidade
{
    public int EspecialidadId { get; set; }

    public string NombreEspecialidad { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Voluntario> Voluntarios { get; set; } = new List<Voluntario>();
}
