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
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uredjaj/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    string VrstaUredjaja,
    StatusUredjaja Status,
    string Naziv,
    double CijenaPoSatu,
    string Model)
        {
            if (VrstaUredjaja == "PC")
            {
                PC pc = new PC
                {
                    Status = Status,
                    Naziv = Naziv,
                    CijenaPoSatu = CijenaPoSatu,
                    Model = Model
                };

                _context.PC.Add(pc);
            }
            else if (VrstaUredjaja == "PlayStation")
            {
                PlayStation ps = new PlayStation
                {
                    Status = Status,
                    Naziv = Naziv,
                    CijenaPoSatu = CijenaPoSatu
                };

                _context.PlayStation.Add(ps);
            }
            else if (VrstaUredjaja == "XBox")
            {
                XBox xbox = new XBox
                {
                    Status = Status,
                    Naziv = Naziv,
                    CijenaPoSatu = CijenaPoSatu
                };

                _context.XBox.Add(xbox);
            }
            else
            {
                ModelState.AddModelError("", "Morate izabrati vrstu uređaja.");
                return View();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Uredjaj/Edit/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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

        // GET: Uredjaj/PrijaviKvar/5
        [Authorize(Roles = "Administrator,Employee")]
        public async Task<IActionResult> PrijaviKvar(int? id)
        {
            if (id == null)
                return NotFound();

            var uredjaj = await _context.Uredjaj.FindAsync(id);

            if (uredjaj == null)
                return NotFound();

            return View(uredjaj);
        }

        // POST: Uredjaj/PrijaviKvar/5
        [Authorize(Roles = "Administrator,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PrijaviKvar(int id, string opis)
        {
            var uredjaj = await _context.Uredjaj.FindAsync(id);

            if (uredjaj == null)
                return NotFound();

            uredjaj.Status = StatusUredjaja.Neispravan;

            Kvar kvar = new Kvar
            {
                Opis = opis,
                Status = StatusKvara.Prijavljen, // ako ti se enum drugačije zove, promijeni ovo
                UredjajId = id
            };

            _context.Kvar.Add(kvar);
            _context.Uredjaj.Update(uredjaj);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Uredjaj/Delete/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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

        public async Task<IActionResult> PC()
        {
            return View(await _context.PC.ToListAsync());
        }

        public async Task<IActionResult> PlayStation()
        {
            return View(await _context.PlayStation.ToListAsync());
        }

        public async Task<IActionResult> XBox()
        {
            return View(await _context.XBox.ToListAsync());
        }
    }
}
