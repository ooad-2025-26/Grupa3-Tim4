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
using System.Security.Claims;

namespace Aplikacija.Controllers
{
    [Authorize(Roles = "Administrator,Employee,User")]
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rezervacija
        .Include(r => r.Korisnik)
        .Include(r => r.Uredjaj);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "Email");
            return View();
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VrijemeOd,VrijemeDo,Status,KorisnikId")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "Id", rezervacija.KorisnikId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "Id", rezervacija.KorisnikId);
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VrijemeOd,VrijemeDo,Status,KorisnikId")] Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.Id))
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
            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "Id", rezervacija.KorisnikId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacija.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.Id == id);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserCreate(DateTime? datum, string deviceType = "PC", int? selectedHour = null)
        {
            DateTime odabraniDatum = datum ?? DateTime.Today;

            List<int> zauzetiSati = new List<int>();

            List<int> uredjajIds = new List<int>();

            if (deviceType == "PC")
                uredjajIds = await _context.PC.Select(x => x.Id).ToListAsync();
            else if (deviceType == "PlayStation")
                uredjajIds = await _context.PlayStation.Select(x => x.Id).ToListAsync();
            else if (deviceType == "XBox")
                uredjajIds = await _context.XBox.Select(x => x.Id).ToListAsync();

            for (int sat = 9; sat <= 21; sat++)
            {
                DateTime vrijemeOd = odabraniDatum.Date.AddHours(sat);
                DateTime vrijemeDo = vrijemeOd.AddHours(1);

                int brojZauzetih = await _context.Rezervacija
                    .Where(r => uredjajIds.Contains(r.UredjajId)
                             && r.VrijemeOd < vrijemeDo
                             && r.VrijemeDo > vrijemeOd)
                    .CountAsync();

                if (uredjajIds.Count == 0 || brojZauzetih >= uredjajIds.Count)
                {
                    zauzetiSati.Add(sat);
                }
            }

            ViewBag.OdabraniDatum = odabraniDatum;
            ViewBag.ZauzetiSati = zauzetiSati;
            ViewBag.SelectedDeviceType = deviceType;
            ViewBag.SelectedHour = selectedHour;

            double cijenaPoSatu = 0;

            if (deviceType == "PC")
                cijenaPoSatu = await _context.PC.Select(x => x.CijenaPoSatu).FirstOrDefaultAsync();
            else if (deviceType == "PlayStation")
                cijenaPoSatu = await _context.PlayStation.Select(x => x.CijenaPoSatu).FirstOrDefaultAsync();
            else if (deviceType == "XBox")
                cijenaPoSatu = await _context.XBox.Select(x => x.CijenaPoSatu).FirstOrDefaultAsync();

            ViewBag.CijenaPoSatu = cijenaPoSatu;
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCreate(DateTime datum, string selectedHours, string deviceType, string paymentMethod)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sati = selectedHours
                .Split(',')
                .Select(int.Parse)
                .ToList();

            foreach (int sat in sati)
            {
                DateTime vrijemeOd = datum.Date.AddHours(sat);
                DateTime vrijemeDo = vrijemeOd.AddHours(1);

                if (vrijemeOd <= DateTime.Now)
                    continue;

                int slobodanUredjajId = 0;

                if (deviceType == "PC")
                {
                    var pcList = await _context.PC.ToListAsync();

                    foreach (var pc in pcList)
                    {
                        bool zauzet = await _context.Rezervacija.AnyAsync(r =>
                            r.UredjajId == pc.Id &&
                            r.VrijemeOd < vrijemeDo &&
                            r.VrijemeDo > vrijemeOd);

                        if (!zauzet)
                        {
                            slobodanUredjajId = pc.Id;
                            break;
                        }
                    }
                }
                else if (deviceType == "PlayStation")
                {
                    var psList = await _context.PlayStation.ToListAsync();

                    foreach (var ps in psList)
                    {
                        bool zauzet = await _context.Rezervacija.AnyAsync(r =>
                            r.UredjajId == ps.Id &&
                            r.VrijemeOd < vrijemeDo &&
                            r.VrijemeDo > vrijemeOd);

                        if (!zauzet)
                        {
                            slobodanUredjajId = ps.Id;
                            break;
                        }
                    }
                }
                else if (deviceType == "XBox")
                {
                    var xboxList = await _context.XBox.ToListAsync();

                    foreach (var xbox in xboxList)
                    {
                        bool zauzet = await _context.Rezervacija.AnyAsync(r =>
                            r.UredjajId == xbox.Id &&
                            r.VrijemeOd < vrijemeDo &&
                            r.VrijemeDo > vrijemeOd);

                        if (!zauzet)
                        {
                            slobodanUredjajId = xbox.Id;
                            break;
                        }
                    }
                }

                if (slobodanUredjajId == 0)
                    continue;

                var rezervacija = new Rezervacija
                {
                    VrijemeOd = vrijemeOd,
                    VrijemeDo = vrijemeDo,
                    KorisnikId = userId,
                    Status = StatusRezervacije.Aktivna,
                    UredjajId = slobodanUredjajId
                };

                _context.Rezervacija.Add(rezervacija);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartSession(int id)
        {
            var rezervacija = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .Include(r => r.Uredjaj)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervacija == null)
                return NotFound();

            var sesija = new Sesija
            {
                VrijemePocetka = DateTime.Now,
                VrijemeZavrsetka = rezervacija.VrijemeDo,
                Status = StatusSesije.Aktivna, // promijeni ako se enum drugačije zove
                KorisnikId = rezervacija.KorisnikId,
                UredjajId = rezervacija.UredjajId
            };

            _context.Sesija.Add(sesija);

            rezervacija.Status = StatusRezervacije.Aktivna; // ili Pokrenuta ako imaš takav status

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
