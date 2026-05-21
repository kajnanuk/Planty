using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aplikacja.Data;
using Aplikacja.Models;
using Aplikacja.Filters;

namespace Aplikacja.Controllers
{
    [Zalogowany]
    public class WymaganieController : Controller
    {
        private readonly AplikacjaContext _context;

        public WymaganieController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: Wymaganie
        public async Task<IActionResult> Index()
        {
            var aplikacjaContext = _context.Wymagania.Include(w => w.Roslina);
            return View(await aplikacjaContext.ToListAsync());
        }

        // GET: Wymaganie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wymaganie = await _context.Wymagania
                .Include(w => w.Roslina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wymaganie == null)
            {
                return NotFound();
            }

            return View(wymaganie);
        }

        // GET: Wymaganie/Create
        public IActionResult Create()
        {
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id");
            return View();
        }

        // POST: Wymaganie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoslinaId,Naslonecznienie,RodzajGleby,ZapotrzebowanieNaWode,Mrozoodpornosc")] Wymaganie wymaganie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wymaganie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id", wymaganie.RoslinaId);
            return View(wymaganie);
        }

        // GET: Wymaganie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wymaganie = await _context.Wymagania.FindAsync(id);
            if (wymaganie == null)
            {
                return NotFound();
            }
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id", wymaganie.RoslinaId);
            return View(wymaganie);
        }

        // POST: Wymaganie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoslinaId,Naslonecznienie,RodzajGleby,ZapotrzebowanieNaWode,Mrozoodpornosc")] Wymaganie wymaganie)
        {
            if (id != wymaganie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wymaganie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WymaganieExists(wymaganie.Id))
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
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id", wymaganie.RoslinaId);
            return View(wymaganie);
        }

        // GET: Wymaganie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wymaganie = await _context.Wymagania
                .Include(w => w.Roslina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wymaganie == null)
            {
                return NotFound();
            }

            return View(wymaganie);
        }

        // POST: Wymaganie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wymaganie = await _context.Wymagania.FindAsync(id);
            if (wymaganie != null)
            {
                _context.Wymagania.Remove(wymaganie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WymaganieExists(int id)
        {
            return _context.Wymagania.Any(e => e.Id == id);
        }
    }
}
