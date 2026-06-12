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


            var istekle = await _context.Rezervacija
    .Where(r => r.Status == StatusRezervacije.Aktivna && r.VrijemeDo < DateTime.Now)
    .ToListAsync();

            foreach (var r in istekle)
            {
                r.Status = StatusRezervacije.Istekla;
            }

            if (istekle.Any())
            {
                await _context.SaveChangesAsync();
            }


            var rezervacije = _context.Rezervacija
                .Include(r => r.Korisnik)
                .Include(r => r.Uredjaj)
                .AsQueryable();

            if (User.IsInRole("User"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                rezervacije = rezervacije.Where(r => r.KorisnikId == userId);
            }

            return View(await rezervacije.ToListAsync());
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .Include(r => r.Uredjaj)
                .Include(r => r.Placanje)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rezervacija == null)
                return NotFound();

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewBag.KorisnikId = new SelectList(_context.Users, "Id", "Email");
            ViewBag.UredjajId = new SelectList(_context.Uredjaj, "Id", "TipUredjaja");

            return View();
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rezervacija rezervacija, string paymentMethod = "Cash")
        {
            if (ModelState.IsValid)
            {
                _context.Rezervacija.Add(rezervacija);
                await _context.SaveChangesAsync();

                double cijenaPoSatu = 0;

                var uredjaj = await _context.Uredjaj.FindAsync(rezervacija.UredjajId);

                if (uredjaj is PC pc)
                    cijenaPoSatu = pc.CijenaPoSatu;
                else if (uredjaj is PlayStation ps)
                    cijenaPoSatu = ps.CijenaPoSatu;
                else if (uredjaj is XBox xbox)
                    cijenaPoSatu = xbox.CijenaPoSatu;

                var trajanje = rezervacija.VrijemeDo - rezervacija.VrijemeOd;
                var ukupno = trajanje.TotalHours * cijenaPoSatu;

                var placanje = new Placanje
                {
                    Iznos = ukupno,
                    MetodaPlacanja = paymentMethod,
                    DatumPlacanja = DateTime.Now,
                    KorisnikId = rezervacija.KorisnikId,
                    RezervacijaId = rezervacija.Id
                };

                _context.Placanje.Add(placanje);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.KorisnikId = new SelectList(_context.Users, "Id", "Email", rezervacija.KorisnikId);
            ViewBag.UredjajId = new SelectList(_context.Uredjaj, "Id", "TipUredjaja", rezervacija.UredjajId);

            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .Include(r => r.Uredjaj)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervacija == null)
                return NotFound();

            ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "Id", "Email", rezervacija.KorisnikId);

            ViewData["UredjajId"] = new SelectList(
                _context.Uredjaj.Select(u => new
                {
                    Id = u.Id,
                    Naziv = u.TipUredjaja + " - ID: " + u.Id
                }),
                "Id",
                "Naziv",
                rezervacija.UredjajId
            );

            ViewData["Status"] = new SelectList(
                Enum.GetValues(typeof(StatusRezervacije))
                    .Cast<StatusRezervacije>()
                    .Select(s => new { Id = s, Naziv = s.ToString() }),
                "Id",
                "Naziv",
                rezervacija.Status
            );

            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var postojeca = await _context.Rezervacija.FindAsync(id);

                if (postojeca == null)
                    return NotFound();

                postojeca.VrijemeOd = rezervacija.VrijemeOd;
                postojeca.VrijemeDo = rezervacija.VrijemeDo;
                postojeca.Status = rezervacija.Status;
                postojeca.KorisnikId = rezervacija.KorisnikId;
                postojeca.UredjajId = rezervacija.UredjajId;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Korisnik)
                .Include(r => r.Uredjaj)
                .Include(r => r.Placanje)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rezervacija == null)
                return NotFound();

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
         && r.Status == StatusRezervacije.Aktivna
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loyalty = await _context.LoyaltyProgram
                .FirstOrDefaultAsync(l => l.KorisnikId == userId);

            ViewBag.LoyaltyPoints = loyalty != null ? loyalty.UkupniBodovi : 0;
            ViewBag.PointsPerKM = 50;

            return View();
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCreate(DateTime datum, string selectedHours, string deviceType, string paymentMethod, int loyaltyPointsToUse)
        { 
            double cijenaPoSatu = 0;

            if (deviceType == "PC")
                cijenaPoSatu = await _context.PC.Select(x => x.CijenaPoSatu).FirstOrDefaultAsync();
            else if (deviceType == "PlayStation")
                cijenaPoSatu = await _context.PlayStation.Select(x => x.CijenaPoSatu).FirstOrDefaultAsync();
            else if (deviceType == "XBox")
                cijenaPoSatu = await _context.XBox.Select(x => x.CijenaPoSatu).FirstOrDefaultAsync();

            int brojSati = selectedHours.Split(',').Count();
            double totalPrice = brojSati * cijenaPoSatu;

            ViewBag.Datum = datum;
            ViewBag.SelectedHours = selectedHours;
            ViewBag.DeviceType = deviceType;
            ViewBag.PaymentMethod = paymentMethod;
            ViewBag.TotalPrice = totalPrice;
            ViewBag.LoyaltyPointsToUse = loyaltyPointsToUse;

            return View("ConfirmPayment");
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

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(
     DateTime datum,
     string selectedHours,
     string deviceType,
     string paymentMethod,
     double totalPrice,
     int loyaltyPointsToUse)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(k => k.Id == identityUserId || k.Email == User.Identity.Name);

            if (korisnik == null)
            {
                TempData["Greska"] = "User was not found in the database.";
                return RedirectToAction("UserCreate");
            }

            var userId = korisnik.Id;

            var sati = selectedHours
                .Split(',')
                .Select(int.Parse)
                .ToList();

            int pointsPerKM = 50;

            var loyalty = await _context.LoyaltyProgram
                .FirstOrDefaultAsync(l => l.KorisnikId == userId);

            double loyaltyDiscount = 0;

            if (loyalty != null && loyaltyPointsToUse >= 50)
            {
                if (loyaltyPointsToUse > loyalty.UkupniBodovi)
                {
                    loyaltyPointsToUse = loyalty.UkupniBodovi;
                }

                loyaltyDiscount = loyaltyPointsToUse / (double)pointsPerKM;

                if (loyaltyDiscount > totalPrice)
                {
                    loyaltyDiscount = totalPrice;
                    loyaltyPointsToUse = (int)(loyaltyDiscount * pointsPerKM);
                }

                loyalty.UkupniBodovi -= loyaltyPointsToUse;
                _context.LoyaltyProgram.Update(loyalty);
            }

            double finalPrice = totalPrice - loyaltyDiscount;

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
                        bool postojiUredjaj = await _context.Uredjaj.AnyAsync(u => u.Id == pc.Id);

                        if (!postojiUredjaj)
                            continue;

                        bool zauzet = await _context.Rezervacija.AnyAsync(r =>
                            r.Status == StatusRezervacije.Aktivna &&
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
                        bool postojiUredjaj = await _context.Uredjaj.AnyAsync(u => u.Id == ps.Id);

                        if (!postojiUredjaj)
                            continue;

                        bool zauzet = await _context.Rezervacija.AnyAsync(r =>
                            r.Status == StatusRezervacije.Aktivna &&
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
                        bool postojiUredjaj = await _context.Uredjaj.AnyAsync(u => u.Id == xbox.Id);

                        if (!postojiUredjaj)
                            continue;

                        bool zauzet = await _context.Rezervacija.AnyAsync(r =>
                            r.Status == StatusRezervacije.Aktivna &&
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
                {
                    TempData["Greska"] = "No available device was found for the selected time.";
                    return RedirectToAction("UserCreate");
                }

                var rezervacija = new Rezervacija
                {
                    VrijemeOd = vrijemeOd,
                    VrijemeDo = vrijemeDo,
                    KorisnikId = userId,
                    Status = StatusRezervacije.Aktivna,
                    UredjajId = slobodanUredjajId
                };

                _context.Rezervacija.Add(rezervacija);
                await _context.SaveChangesAsync();

                var placanje = new Placanje
                {
                    Iznos = finalPrice / sati.Count,
                    MetodaPlacanja = paymentMethod,
                    DatumPlacanja = DateTime.Now,
                    KorisnikId = userId,
                    RezervacijaId = rezervacija.Id
                };

                _context.Placanje.Add(placanje);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(r => r.Id == id && r.KorisnikId == userId);

            if (rezervacija == null)
                return NotFound();

            if (rezervacija.Status == StatusRezervacije.Aktivna && rezervacija.VrijemeOd > DateTime.Now)
            {
                rezervacija.Status = StatusRezervacije.Otkazana;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
