using Aplikacija.Models;
using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aplikacija.Controllers
{
    [Authorize(Roles = "Administrator,Employee")]
    public class TakmicenjeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public TakmicenjeController(
    ApplicationDbContext context,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: Takmicenje
        public async Task<IActionResult> Index()
        {
            return View(await _context.Takmicenje.ToListAsync());
        }

        // GET: Takmicenje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicenje = await _context.Takmicenje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicenje == null)
            {
                return NotFound();
            }

            return View(takmicenje);
        }

        // GET: Takmicenje/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Takmicenje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Igra,Datum")] Takmicenje takmicenje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(takmicenje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(takmicenje);
        }

        // GET: Takmicenje/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicenje = await _context.Takmicenje.FindAsync(id);
            if (takmicenje == null)
            {
                return NotFound();
            }
            return View(takmicenje);
        }

        // POST: Takmicenje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Igra,Datum")] Takmicenje takmicenje)
        {
            if (id != takmicenje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(takmicenje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TakmicenjeExists(takmicenje.Id))
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
            return View(takmicenje);
        }

        // GET: Takmicenje/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicenje = await _context.Takmicenje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicenje == null)
            {
                return NotFound();
            }

            return View(takmicenje);
        }

        // POST: Takmicenje/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var takmicenje = await _context.Takmicenje.FindAsync(id);
            if (takmicenje != null)
            {
                _context.Takmicenje.Remove(takmicenje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TakmicenjeExists(int id)
        {
            return _context.Takmicenje.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UcitajIzVanjskogSistema()
        {
            string apiUrl = _configuration["PandaScore:ApiUrl"];
            string token = _configuration["PandaScore:Token"];

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var json = await client.GetStringAsync(apiUrl);

            var podaci = JsonSerializer.Deserialize<List<PandaScoreMatch>>(json);

            if (podaci == null || podaci.Count == 0)
            {
                TempData["Poruka"] = "Vanjski sistem trenutno nije vratio nijedno takmičenje.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var item in podaci)
            {
                DateTime datum = DateTime.Now;

                if (item.begin_at != null)
                {
                    DateTime.TryParse(item.begin_at, out datum);
                }

                bool postoji = await _context.Takmicenje.AnyAsync(t =>
                    t.Naziv == item.name &&
                    t.Datum.Date == datum.Date
                );

                if (!postoji)
                {
                    var takmicenje = new Takmicenje
                    {
                        Naziv = item.name ?? item.league?.name ?? "Esports Match",
                        Igra = item.videogame?.name ?? "Game",
                        Datum = datum
                    };

                    _context.Takmicenje.Add(takmicenje);
                }
            }

            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Takmičenja su učitana iz PandaScore vanjskog sistema.";

            return RedirectToAction(nameof(Index));
        }
    }
}
