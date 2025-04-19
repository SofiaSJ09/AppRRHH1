using System;
using System.Collections.Generic;

namespace AppRRHH1.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public int PuestoTrabajoId { get; set; }

    public virtual ICollection<Jornada> Jornada { get; set; } = new List<Jornada>();

    public virtual PuestoTrabajo PuestoTrabajo { get; set; } = null!;
}
