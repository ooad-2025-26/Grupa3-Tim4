using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aplikacija.Models;

namespace Aplikacija.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var korisnik = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (korisnik == null) return NotFound();

            return View(korisnik);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Email,PhoneNumber,ClanarinaAktivna")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(korisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(korisnik);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var korisnik = await _context.Users.FindAsync(id);

            if (korisnik == null) return NotFound();

            return View(korisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PhoneNumber,ClanarinaAktivna")] Korisnik korisnik)
        {
            if (id != korisnik.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Update(korisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikExists(korisnik.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(korisnik);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var korisnik = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (korisnik == null) return NotFound();

            return View(korisnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnik = await _context.Users.FindAsync(id);

            if (korisnik != null)
            {
                _context.Users.Remove(korisnik);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}