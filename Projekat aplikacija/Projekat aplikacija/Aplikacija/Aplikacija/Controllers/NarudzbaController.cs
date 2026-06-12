using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class NarudzbaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NarudzbaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> Index()
        {
            var narudzbe = await _context.Narudzba
                .Include(n => n.Korisnik)
                .Include(n => n.Sesija)
                    .ThenInclude(s => s.Uredjaj)
                .OrderByDescending(n => n.VrijemeSlanja)
                .ToListAsync();

            return View(narudzbe);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickSend(TipNarudzbe tip, string? proizvod, string? poruka)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sada = DateTime.Now;

            var aktivnaSesija = await _context.Sesija
                .FirstOrDefaultAsync(s => s.KorisnikId == userId
                                       && s.Status == StatusSesije.Aktivna
                                       && s.VrijemePocetka <= sada
                                       && s.VrijemeZavrsetka >= sada);

            if (aktivnaSesija == null)
            {
                TempData["Greska"] = "You can send orders or messages only during an active session.";
                return RedirectToAction("Index", "Home");
            }

            if (tip == TipNarudzbe.Narudzba && string.IsNullOrWhiteSpace(proizvod))
            {
                TempData["Greska"] = "Enter what you want to order.";
                return RedirectToAction("Index", "Home");
            }

            if (tip == TipNarudzbe.Poruka && string.IsNullOrWhiteSpace(poruka))
            {
                TempData["Greska"] = "Enter a message.";
                return RedirectToAction("Index", "Home");
            }

            var narudzba = new Narudzba
            {
                Tip = tip,
                Proizvod = tip == TipNarudzbe.Narudzba ? proizvod : null,
                Poruka = tip == TipNarudzbe.Poruka ? poruka : null,
                VrijemeSlanja = DateTime.Now,
                KorisnikId = userId,
                SesijaId = aktivnaSesija.Id
            };

            _context.Narudzba.Add(narudzba);
            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Your request has been sent.";
            return RedirectToAction("Index", "Home");
        }
    }
}