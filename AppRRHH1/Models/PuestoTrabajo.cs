using System;
using System.Collections.Generic;

namespace AppRRHH1.Models;

public partial class PuestoTrabajo
{
    public int PuestoTrabajoId { get; set; }

    public string? Nombre { get; set; }

    public decimal? PagoHora { get; set; }

    public int? DepartamentoId { get; set; }

    public virtual Departamento? Departamento { get; set; }

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
