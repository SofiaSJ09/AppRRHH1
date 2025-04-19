using System;
using System.Collections.Generic;

namespace AppRRHH1.Models;

public partial class Departamento
{
    public int DepartamentoId { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<PuestoTrabajo> PuestoTrabajos { get; set; } = new List<PuestoTrabajo>();
}
