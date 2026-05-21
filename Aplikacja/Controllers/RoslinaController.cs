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
    public class RoslinaController : Controller
    {
        private readonly AplikacjaContext _context;

        public RoslinaController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: Roslina
        public async Task<IActionResult> Index()
        {
            var aplikacjaContext = _context.Rosliny.Include(r => r.Warstwa);
            return View(await aplikacjaContext.ToListAsync());
        }

        // GET: Roslina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roslina = await _context.Rosliny
                .Include(r => r.Warstwa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roslina == null)
            {
                return NotFound();
            }

            return View(roslina);
        }

        // GET: Roslina/Create
        public IActionResult Create()
        {
            ViewData["WarstwaId"] = new SelectList(_context.Warstwy, "Id", "Nazwa");
            return View();
        }

        // POST: Roslina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NazwaPolska,NazwaLacinska,Opis,WarstwaId")] Roslina roslina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roslina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WarstwaId"] = new SelectList(_context.Warstwy, "Id", "Id", roslina.WarstwaId);
            return View(roslina);
        }

        // GET: Roslina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roslina = await _context.Rosliny.FindAsync(id);
            if (roslina == null)
            {
                return NotFound();
            }
            ViewData["WarstwaId"] = new SelectList(_context.Warstwy, "Id", "Nazwa", roslina.WarstwaId);
            return View(roslina);
        }

        // POST: Roslina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NazwaPolska,NazwaLacinska,Opis,WarstwaId")] Roslina roslina)
        {
            if (id != roslina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roslina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoslinaExists(roslina.Id))
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
            ViewData["WarstwaId"] = new SelectList(_context.Warstwy, "Id", "Id", roslina.WarstwaId);
            return View(roslina);
        }

        // GET: Roslina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roslina = await _context.Rosliny
                .Include(r => r.Warstwa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roslina == null)
            {
                return NotFound();
            }

            return View(roslina);
        }

        // POST: Roslina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roslina = await _context.Rosliny.FindAsync(id);
            if (roslina != null)
            {
                _context.Rosliny.Remove(roslina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoslinaExists(int id)
        {
            return _context.Rosliny.Any(e => e.Id == id);
        }
    }
}
