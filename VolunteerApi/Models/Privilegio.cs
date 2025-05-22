using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class Privilegio
{
    public int PrivilegioId { get; set; }

    public string NombrePrivilegio { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Role> Rols { get; set; } = new List<Role>();
}
