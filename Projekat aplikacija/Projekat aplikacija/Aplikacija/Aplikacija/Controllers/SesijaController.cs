using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aplikacija.Models;

namespace Aplikacija.Controllers
{
    [Authorize(Roles = "Administrator,Employee")]
    public class SesijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SesijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sesija
        public async Task<IActionResult> Index()
        {
            var sada = DateTime.Now;

            var istekleSesije = await _context.Sesija
                .Where(s => s.Status == StatusSesije.Aktivna
                         && s.VrijemeZavrsetka <= sada)
                .ToListAsync();

            foreach (var sesija in istekleSesije)
            {
                sesija.Status = StatusSesije.Zavrsena;
            }

            if (istekleSesije.Any())
            {
                await _context.SaveChangesAsync();
            }

            var applicationDbContext = _context.Sesija
                .Include(s => s.Uredjaj)
                .Include(s => s.Korisnik);

            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Sesija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesija = await _context.Sesija
                .Include(s => s.Uredjaj).Include(s => s.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sesija == null)
            {
                return NotFound();
            }

            return View(sesija);
        }

        // GET: Sesija/Create
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Create()
        {
            var sada = DateTime.Now;

            var aktivniUredjaji = await _context.Sesija
                .Where(s => s.Status == StatusSesije.Aktivna)
                .Select(s => s.UredjajId)
                .ToListAsync();

            var rezervacije = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .Include(r => r.Uredjaj)
                .Include(r => r.Placanje)
                .Where(r => r.Status == StatusRezervacije.Aktivna
                         && r.VrijemeOd <= sada
                         && r.VrijemeDo >= sada)
                .ToListAsync();

            var rezervisaniUredjaji = rezervacije
                .Select(r => r.UredjajId)
                .ToList();

            var dostupniUredjaji = await _context.Uredjaj
                .Where(u => !aktivniUredjaji.Contains(u.Id)
                         && !rezervisaniUredjaji.Contains(u.Id))
                .ToListAsync();

            ViewBag.Rezervacije = rezervacije;

            ViewBag.Uredjaji = dostupniUredjaji.Select(u => new
            {
                Id = u.Id,
                Naziv = u.TipUredjaja + " - ID: " + u.Id,
                CijenaPoSatu =
        u is PC pc ? pc.CijenaPoSatu :
        u is PlayStation ps ? ps.CijenaPoSatu :
        u is XBox xbox ? xbox.CijenaPoSatu : 0
            }).ToList();

            ViewBag.KorisnikId = new SelectList(
                await _context.Korisnik.ToListAsync(),
                "Id",
                "Email"
            );

            return View(new Sesija());
        }

        // POST: Sesija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    int? rezervacijaId,
    int? uredjajId,
    string? korisnikId,
    bool anonimniGost,
    string vrijemeZavrsetkaVrijeme)
        {
            
            var sada = DateTime.Now;
            var vrijemePocetka = DateTime.Now;

            var vrijeme = TimeSpan.Parse(vrijemeZavrsetkaVrijeme);

            var vrijemeZavrsetka = DateTime.Today.Add(vrijeme);

            if (vrijemeZavrsetka <= vrijemePocetka)
            {
                ModelState.AddModelError("", "End time must be after start time.");
            }

            if (rezervacijaId != null)
            {
                var rezervacija = await _context.Rezervacija
                    .Include(r => r.Korisnik)
                    .Include(r => r.Uredjaj)
                    .FirstOrDefaultAsync(r => r.Id == rezervacijaId);

                if (rezervacija == null)
                {
                    return NotFound();
                }

                var sesijaIzRezervacije = new Sesija
                {
                    VrijemePocetka = vrijemePocetka,
                    VrijemeZavrsetka = vrijemeZavrsetka,
                    Status = StatusSesije.Aktivna,
                    UredjajId = rezervacija.UredjajId,
                    KorisnikId = rezervacija.KorisnikId,
                    AnonimniGost = false
                };

                _context.Sesija.Add(sesijaIzRezervacije);

                rezervacija.Status = StatusRezervacije.Zavrsena;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            if (uredjajId == null)
            {
                ModelState.AddModelError("", "You must select a device.");
            }

            if (!anonimniGost && string.IsNullOrEmpty(korisnikId))
            {
                ModelState.AddModelError("", "Select a user or mark the guest as anonymous.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }

            var sesija = new Sesija
            {
                VrijemePocetka = vrijemePocetka,
                VrijemeZavrsetka = vrijemeZavrsetka,
                Status = StatusSesije.Aktivna,
                UredjajId = uredjajId.Value,
                KorisnikId = anonimniGost ? null : korisnikId,
                AnonimniGost = anonimniGost
            };

            _context.Sesija.Add(sesija);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Sesija/Edit/5
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesija = await _context.Sesija.FindAsync(id);
            if (sesija == null)
            {
                return NotFound();
            }
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj.Select(u => new
                {
                    Id = u.Id,
                    Naziv = u.TipUredjaja + " - ID: " + u.Id
                }),
                "Id",
                "Naziv",
                sesija.UredjajId


            );

            ViewData["KorisnikId"] = new SelectList(
    _context.Korisnik,
    "Id",
    "Email",
    sesija.KorisnikId
);
            return View(sesija);
        }



        // POST: Sesija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VrijemePocetka,VrijemeZavrsetka,Status,UredjajId,KorisnikId,AnonimniGost")] Sesija sesija)
        {
            if (id != sesija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sesija.AnonimniGost = string.IsNullOrEmpty(sesija.KorisnikId);
                    _context.Update(sesija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SesijaExists(sesija.Id))
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
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id", sesija.UredjajId);
            return View(sesija);
        }

        // GET: Sesija/Delete/5
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesija = await _context.Sesija
                .Include(s => s.Uredjaj).Include(s => s.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sesija == null)
            {
                return NotFound();
            }

            return View(sesija);
        }

        // POST: Sesija/Delete/5
        [Authorize(Roles = "Administrator,Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sesija = await _context.Sesija.FindAsync(id);
            if (sesija != null)
            {
                _context.Sesija.Remove(sesija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SesijaExists(int id)
        {
            return _context.Sesija.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Administrator,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndSession(int id)
        {
            var sesija = await _context.Sesija.FindAsync(id);

            if (sesija == null)
            {
                return NotFound();
            }

            sesija.Status = StatusSesije.Zavrsena;
            sesija.VrijemeZavrsetka = DateTime.Now;

            if (!string.IsNullOrEmpty(sesija.KorisnikId))
            {
                int pointsToAdd = 10;

                var loyalty = await _context.LoyaltyProgram
                    .FirstOrDefaultAsync(l => l.KorisnikId == sesija.KorisnikId);

                if (loyalty == null)
                {
                    loyalty = new LoyaltyProgram
                    {
                        KorisnikId = sesija.KorisnikId,
                        UkupniBodovi = pointsToAdd,
                        PopustPoBodu = 0.10
                    };

                    _context.LoyaltyProgram.Add(loyalty);
                }
                else
                {
                    loyalty.UkupniBodovi += pointsToAdd;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
