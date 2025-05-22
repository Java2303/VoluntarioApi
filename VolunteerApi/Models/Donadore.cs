using System;
using System.Collections.Generic;

namespace VolunteerApi.Models;

public partial class Donadore
{
    public int DonadorId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string CiNit { get; set; } = null!;

    public string? NumeroReferencia { get; set; }
}
