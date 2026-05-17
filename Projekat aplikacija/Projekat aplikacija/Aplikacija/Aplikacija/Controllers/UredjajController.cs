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
    public class UredjajController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UredjajController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Uredjaj
        public async Task<IActionResult> Index()
        {
            return View(await _context.Uredjaj.ToListAsync());
        }

        // GET: Uredjaj/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uredjaj = await _context.Uredjaj
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uredjaj == null)
            {
                return NotFound();
            }

            return View(uredjaj);
        }

        // GET: Uredjaj/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uredjaj/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status")] Uredjaj uredjaj)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uredjaj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uredjaj);
        }

        // GET: Uredjaj/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uredjaj = await _context.Uredjaj.FindAsync(id);
            if (uredjaj == null)
            {
                return NotFound();
            }
            return View(uredjaj);
        }

        // POST: Uredjaj/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] Uredjaj uredjaj)
        {
            if (id != uredjaj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uredjaj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UredjajExists(uredjaj.Id))
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
            return View(uredjaj);
        }

        // GET: Uredjaj/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uredjaj = await _context.Uredjaj
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uredjaj == null)
            {
                return NotFound();
            }

            return View(uredjaj);
        }

        // POST: Uredjaj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uredjaj = await _context.Uredjaj.FindAsync(id);
            if (uredjaj != null)
            {
                _context.Uredjaj.Remove(uredjaj);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UredjajExists(int id)
        {
            return _context.Uredjaj.Any(e => e.Id == id);
        }
    }
}
