using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class Role
{
    public int RolId { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Privilegio> Privilegios { get; set; } = new List<Privilegio>();
}
