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
    public class PuestoTrabajoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PuestoTrabajoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PuestoTrabajo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PuestoTrabajos.Include(p => p.Departamento);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PuestoTrabajo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestoTrabajo = await _context.PuestoTrabajos
                .Include(p => p.Departamento)
                .FirstOrDefaultAsync(m => m.PuestoTrabajoId == id);
            if (puestoTrabajo == null)
            {
                return NotFound();
            }

            return View(puestoTrabajo);
        }

        // GET: PuestoTrabajo/Create
        public IActionResult Create()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nombre");
            return View();
        }

        // POST: PuestoTrabajo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PuestoTrabajoId,Nombre,PagoHora,DepartamentoId")] PuestoTrabajo puestoTrabajo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puestoTrabajo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "DepartamentoId", puestoTrabajo.DepartamentoId);
            return View(puestoTrabajo);
        }

        // GET: PuestoTrabajo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestoTrabajo = await _context.PuestoTrabajos.FindAsync(id);
            if (puestoTrabajo == null)
            {
                return NotFound();
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "DepartamentoId", puestoTrabajo.DepartamentoId);
            return View(puestoTrabajo);
        }

        // POST: PuestoTrabajo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PuestoTrabajoId,Nombre,PagoHora,DepartamentoId")] PuestoTrabajo puestoTrabajo)
        {
            if (id != puestoTrabajo.PuestoTrabajoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puestoTrabajo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestoTrabajoExists(puestoTrabajo.PuestoTrabajoId))
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
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "DepartamentoId", puestoTrabajo.DepartamentoId);
            return View(puestoTrabajo);
        }

        // GET: PuestoTrabajo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestoTrabajo = await _context.PuestoTrabajos
                .Include(p => p.Departamento)
                .FirstOrDefaultAsync(m => m.PuestoTrabajoId == id);
            if (puestoTrabajo == null)
            {
                return NotFound();
            }

            return View(puestoTrabajo);
        }

        // POST: PuestoTrabajo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var puestoTrabajo = await _context.PuestoTrabajos.FindAsync(id);
            if (puestoTrabajo != null)
            {
                _context.PuestoTrabajos.Remove(puestoTrabajo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuestoTrabajoExists(int id)
        {
            return _context.PuestoTrabajos.Any(e => e.PuestoTrabajoId == id);
        }
    }
}
