using AppRRHH1.Data;
using AppRRHH1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace AppRRHH1.Controllers
{
    [Authorize(Roles = Constantes.Admin)]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            var clainsIdentity = (ClaimsIdentity)this.User.Identity;
            var userActual = clainsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(await _context.Users.Where(u => u.Id !=
           userActual.Value).ToListAsync());
        }
        // GET: UsuariosController/Details/5
        public async Task<ActionResult> Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.LockoutEnd = DateTime.UtcNow.AddYears(1000);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        // GET: UsuariosController/Details/5
        public async Task<ActionResult> Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.LockoutEnd = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
