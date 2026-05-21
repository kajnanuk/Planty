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
    public class WarstwaController : Controller
    {
        private readonly AplikacjaContext _context;

        public WarstwaController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: Warstwa
        public async Task<IActionResult> Index()
        {
            return View(await _context.Warstwy.ToListAsync());
        }

        // GET: Warstwa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warstwa = await _context.Warstwy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warstwa == null)
            {
                return NotFound();
            }

            return View(warstwa);
        }

        // GET: Warstwa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warstwa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Opis")] Warstwa warstwa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warstwa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warstwa);
        }

        // GET: Warstwa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warstwa = await _context.Warstwy.FindAsync(id);
            if (warstwa == null)
            {
                return NotFound();
            }
            return View(warstwa);
        }

        // POST: Warstwa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Opis")] Warstwa warstwa)
        {
            if (id != warstwa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warstwa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarstwaExists(warstwa.Id))
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
            return View(warstwa);
        }

        // GET: Warstwa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warstwa = await _context.Warstwy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warstwa == null)
            {
                return NotFound();
            }

            return View(warstwa);
        }

        // POST: Warstwa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warstwa = await _context.Warstwy.FindAsync(id);
            if (warstwa != null)
            {
                _context.Warstwy.Remove(warstwa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarstwaExists(int id)
        {
            return _context.Warstwy.Any(e => e.Id == id);
        }
    }
}
