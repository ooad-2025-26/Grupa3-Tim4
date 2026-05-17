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
            var applicationDbContext = _context.Sesija.Include(s => s.Uredjaj);
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
                .Include(s => s.Uredjaj)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sesija == null)
            {
                return NotFound();
            }

            return View(sesija);
        }

        // GET: Sesija/Create
        public IActionResult Create()
        {
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id");
            return View();
        }

        // POST: Sesija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VrijemePocetka,VrijemeZavrsetka,Status,UredjajId")] Sesija sesija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sesija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id", sesija.UredjajId);
            return View(sesija);
        }

        // GET: Sesija/Edit/5
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
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id", sesija.UredjajId);
            return View(sesija);
        }

        // POST: Sesija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VrijemePocetka,VrijemeZavrsetka,Status,UredjajId")] Sesija sesija)
        {
            if (id != sesija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesija = await _context.Sesija
                .Include(s => s.Uredjaj)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sesija == null)
            {
                return NotFound();
            }

            return View(sesija);
        }

        // POST: Sesija/Delete/5
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
    }
}
