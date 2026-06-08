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
    public class KvarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KvarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kvar
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Kvar.Include(k => k.Uredjaj);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Kvar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kvar = await _context.Kvar
                .Include(k => k.Uredjaj)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kvar == null)
            {
                return NotFound();
            }

            return View(kvar);
        }

        // GET: Kvar/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id");
            return View();
        }

        // POST: Kvar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Opis,Status,UredjajId")] Kvar kvar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kvar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id", kvar.UredjajId);
            return View(kvar);
        }

        // GET: Kvar/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kvar = await _context.Kvar.FindAsync(id);
            if (kvar == null)
            {
                return NotFound();
            }
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id", kvar.UredjajId);
            return View(kvar);
        }

        // POST: Kvar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Opis,Status,UredjajId")] Kvar kvar)
        {
            if (id != kvar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kvar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KvarExists(kvar.Id))
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
            ViewData["UredjajId"] = new SelectList(_context.Uredjaj, "Id", "Id", kvar.UredjajId);
            return View(kvar);
        }

        // GET: Kvar/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kvar = await _context.Kvar
                .Include(k => k.Uredjaj)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kvar == null)
            {
                return NotFound();
            }

            return View(kvar);
        }

        // POST: Kvar/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kvar = await _context.Kvar.FindAsync(id);
            if (kvar != null)
            {
                _context.Kvar.Remove(kvar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KvarExists(int id)
        {
            return _context.Kvar.Any(e => e.Id == id);
        }
    }
}
