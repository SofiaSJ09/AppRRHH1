using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppRRHH1.Data;
using AppRRHH1.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppRRHH1.Controllers
{
    [Authorize]
    public class EmpleadoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empleado
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Empleados.Include(e => e.PuestoTrabajo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Empleado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.PuestoTrabajo)
                .FirstOrDefaultAsync(m => m.EmpleadoId == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleado/Create
        public IActionResult Create()
        {
            ViewData["PuestoTrabajoId"] = new SelectList(_context.PuestoTrabajos, "PuestoTrabajoId", "Nombre");
            return View();
        }

        // POST: Empleado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpleadoId,Nombre,Apellidos,Correo,Telefono,PuestoTrabajoId")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PuestoTrabajoId"] = new SelectList(_context.PuestoTrabajos, "PuestoTrabajoId", "PuestoTrabajoId", empleado.PuestoTrabajoId);
            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            ViewData["PuestoTrabajoId"] = new SelectList(_context.PuestoTrabajos, "PuestoTrabajoId", "PuestoTrabajoId", empleado.PuestoTrabajoId);
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpleadoId,Nombre,Apellidos,Correo,Telefono,PuestoTrabajoId")] Empleado empleado)
        {
            if (id != empleado.EmpleadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.EmpleadoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PuestoTrabajoId"] = new SelectList(_context.PuestoTrabajos, "PuestoTrabajoId", "PuestoTrabajoId", empleado.PuestoTrabajoId);
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.PuestoTrabajo)
                .FirstOrDefaultAsync(m => m.EmpleadoId == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.EmpleadoId == id);
        }
    }
}
