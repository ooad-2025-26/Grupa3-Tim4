using Aplikacija.Models;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public IActionResult Pocetna()
        {
            if (HttpContext.User.IsInRole("Administrator"))
                return View("AdminPanel");

            if (HttpContext.User.IsInRole("Employee"))
                return View("EmployeePanel");

            return View("UserPanel");
        }
        public KorisnikController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Korisnik


        public async Task<IActionResult> Index()
        {
            var korisnici = await _context.Korisnik.ToListAsync();

            ViewBag.LoyaltyPrograms = await _context.LoyaltyProgram.ToListAsync();

            return View(korisnici);
        }

        // GET: Korisnik/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(k => k.Id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            var loyalty = await _context.LoyaltyProgram
    .FirstOrDefaultAsync(l => l.KorisnikId == korisnik.Id);

            ViewBag.LoyaltyPoints = loyalty?.UkupniBodovi ?? 0;

            return View(korisnik);
        }

        // GET: Korisnik/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Korisnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string ime, string prezime, string email, string password, bool clanarinaAktivna)
        {
            var korisnik = new Korisnik
            {
                Ime = ime,
                Prezime = prezime,
                Email = email,
                UserName = email,
                ClanarinaAktivna = clanarinaAktivna
            };

            var result = await _userManager.CreateAsync(korisnik, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(korisnik, "User");
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(korisnik);
        }

        // GET: Korisnik/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik.FindAsync(id);
            if (korisnik == null)
            {
                return NotFound();
            }
            return View(korisnik);
        }

        

        // POST: Korisnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string ime, string prezime, string email, bool clanarinaAktivna)
        {
            var korisnik = await _context.Korisnik.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            korisnik.Ime = ime;
            korisnik.Prezime = prezime;
            korisnik.Email = email;
            korisnik.UserName = email;
            korisnik.ClanarinaAktivna = clanarinaAktivna;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Korisnik/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: Korisnik/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnik = await _context.Korisnik.FindAsync(id);
            if (korisnik != null)
            {
                _context.Korisnik.Remove(korisnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLoyaltyPoints(string korisnikId, int points)
        {
            if (points <= 0)
                return RedirectToAction(nameof(Index));

            var loyalty = await _context.LoyaltyProgram
                .FirstOrDefaultAsync(l => l.KorisnikId == korisnikId);

            if (loyalty == null)
            {
                loyalty = new LoyaltyProgram
                {
                    KorisnikId = korisnikId,
                    UkupniBodovi = points,
                    PopustPoBodu = 0.04
                };

                _context.LoyaltyProgram.Add(loyalty);
            }
            else
            {
                loyalty.UkupniBodovi += points;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLoyaltyPoints(string korisnikId, int points)
        {
            if (points <= 0)
                return RedirectToAction(nameof(Index));

            var loyalty = await _context.LoyaltyProgram
                .FirstOrDefaultAsync(l => l.KorisnikId == korisnikId);

            if (loyalty != null)
            {
                loyalty.UkupniBodovi -= points;

                if (loyalty.UkupniBodovi < 0)
                    loyalty.UkupniBodovi = 0;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikExists(string id)
        {
            return _context.Korisnik.Any(e => e.Id == id);
        }
    }
}
