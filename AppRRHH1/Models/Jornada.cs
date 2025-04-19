using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppRRHH1.Models;

public partial class Jornada
{
    public int JornadaId { get; set; }

    public int EmpleadoId { get; set; }
    [DataType(DataType.Date)]
    public DateOnly FechaInicio { get; set; }
    [DataType(DataType.Date)]
    public DateOnly FechaFin { get; set; }

    public decimal HorasTrabajadas { get; set; }

    public decimal SalarioBruto { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;
}
