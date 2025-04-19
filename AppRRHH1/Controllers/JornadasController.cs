using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppRRHH1.Data;
using AppRRHH1.Models;
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using AppRRHH1.Data;
using AppRRHH1.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AppRRHH.Controllers
{
    [Authorize]
    public class JornadasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JornadasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jornadas
        public async Task<IActionResult> Index()
        {
            var empleados = _context.Empleados.Include(j => j.PuestoTrabajo);
            return View("Lista", await empleados.ToListAsync());
        }

        // GET: Jornadas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornada = await _context.Empleados
                .Include(p => p.PuestoTrabajo)
                .Include(j => j.Jornada)
                .FirstOrDefaultAsync(e => e.EmpleadoId == id);
            if (jornada == null)
            {
                return NotFound();
            }

            return View("Ver", jornada);
        }

        // GET: Jornadas/Create
        public async Task<IActionResult> Create(int? id)
        {
            var empleado = await _context.Empleados
                 .Include(e => e.PuestoTrabajo)
                 .FirstOrDefaultAsync(m => m.EmpleadoId == id);
            ViewData["EmpleadoId"] = empleado.EmpleadoId;
            ViewData["Nombre"] = empleado.Nombre + " " + empleado.Apellidos;
            ViewData["PagoHora"] = empleado.PuestoTrabajo.PagoHora;
            ViewData["Puesto"] = empleado.PuestoTrabajo.Nombre;

            return View();
        }

        // POST: Jornadas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Jornada jornada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jornada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = jornada.EmpleadoId });
            }
            return View(jornada);
        }

        // GET: Jornadas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornada = await _context.Jornadas
                .Include(j => j.Empleado).ThenInclude(p => p.PuestoTrabajo)
                .FirstOrDefaultAsync(m => m.JornadaId == id);
            if (jornada == null)
            {
                return NotFound();
            }
            return View("Editar", jornada);
        }

        // POST: Jornadas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Jornada jornada)
        {
            if (id != jornada.JornadaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jornada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JornadaExists(jornada.JornadaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = jornada.EmpleadoId });
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "EmpleadoId", jornada.EmpleadoId);
            return View(jornada);
        }

        // GET: Jornadas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornada = await _context.Jornadas
                 .Include(j => j.Empleado).ThenInclude(p => p.PuestoTrabajo)
                 .FirstOrDefaultAsync(m => m.JornadaId == id);
            if (jornada == null)
            {
                return NotFound();
            }

            return View(jornada);
        }

        // POST: Jornadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jornada = await _context.Jornadas.FindAsync(id);
            if (jornada != null)
            {
                _context.Jornadas.Remove(jornada);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = jornada.EmpleadoId });
        }

        private bool JornadaExists(int id)
        {
            return _context.Jornadas.Any(e => e.JornadaId == id);
        }

        //Exportar a Excel Empleados
        [HttpGet]
        public async Task<FileResult> ExportarEmpleadosAExcel()
        {
            var empleados = _context.Empleados.Include(e => e.PuestoTrabajo);
            var nombreArchivo = $"Empleados.xlsx";
            return GenerarExcel(nombreArchivo, empleados);
        }


        private FileResult GenerarExcel(string nombreArchivo, IEnumerable<Empleado> empleados)
        {
            DataTable dataTable = new DataTable("Personas");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("EmpleadoId"),
                new DataColumn("Nombre"),
                new DataColumn("Apellidos"),
                new DataColumn("Correo"),
                new DataColumn("Telefono"),
                new DataColumn("PuetoTrabajo"),
                new DataColumn("PagoHora")

            });

            foreach (var emp in empleados)
            {
                dataTable.Rows.Add(emp.EmpleadoId,
                    emp.Nombre,
                    emp.Apellidos,
                    emp.Correo,
                    emp.Telefono,
                    emp.PuestoTrabajo.Nombre,
                    emp.PuestoTrabajo.PagoHora);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo);
                }
            }

        }
        //Exportar a Excel Empleado y sus jornadas
        [HttpGet]
        public async Task<FileResult> ExportarEmpleadosJornadasAExcel(int? id)
        {
            var empleado = await _context.Empleados
                 .Include(p => p.PuestoTrabajo)
                 .Include(j => j.Jornada)
                 .FirstOrDefaultAsync(e => e.EmpleadoId == id);
            var nombreArchivo = $"Jornadas.xlsx";
            return GenerarExcelJornada(nombreArchivo, empleado);
        }


        private FileResult GenerarExcelJornada(string nombreArchivo, Empleado empleado)
        {
            DataTable dataTable = new DataTable("Jornadas");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("EmpleadoId"),
                new DataColumn("Nombre"),
                new DataColumn("Apellidos"),
                new DataColumn("PuetoTrabajo"),
                new DataColumn("PagoHora")

            });

            dataTable.Rows.Add(
                empleado.EmpleadoId,
                empleado.Nombre,
                empleado.Apellidos,
                empleado.PuestoTrabajo.Nombre,
                empleado.PuestoTrabajo.PagoHora
                );

            dataTable.Rows.Add(
                "FechaInicio",
                "FechaFin",
                "HorasTrabajadas",
                "SalarioBruto"
           );

            foreach (var jornada in empleado.Jornada)
            {
                dataTable.Rows.Add(
                    jornada.FechaInicio,
                    jornada.FechaFin,
                    jornada.HorasTrabajadas,
                    jornada.SalarioBruto);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo);
                }
            }
        }
    }
}
