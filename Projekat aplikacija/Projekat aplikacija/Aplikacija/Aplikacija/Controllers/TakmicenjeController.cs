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
    public class TakmicenjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TakmicenjeController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
