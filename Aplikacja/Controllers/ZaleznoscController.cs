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
    public class ZaleznoscController : Controller
    {
        private readonly AplikacjaContext _context;

        public ZaleznoscController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: Zaleznosc
        public async Task<IActionResult> Index()
        {
            var aplikacjaContext = _context.Zaleznosci.Include(z => z.RoslinaGlowna).Include(z => z.RoslinaTowarzyszaca);
            return View(await aplikacjaContext.ToListAsync());
        }

        // GET: Zaleznosc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaleznosc = await _context.Zaleznosci
                .Include(z => z.RoslinaGlowna)
                .Include(z => z.RoslinaTowarzyszaca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaleznosc == null)
            {
                return NotFound();
            }

            return View(zaleznosc);
        }

        // GET: Zaleznosc/Create
        public IActionResult Create()
        {
            ViewData["RoslinaGlownaId"] = new SelectList(_context.Rosliny, "Id", "Id");
            ViewData["RoslinaTowarzyszacaId"] = new SelectList(_context.Rosliny, "Id", "Id");
            return View();
        }

        // POST: Zaleznosc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoslinaGlownaId,RoslinaTowarzyszacaId,TypZaleznosci,Opis")] Zaleznosc zaleznosc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zaleznosc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoslinaGlownaId"] = new SelectList(_context.Rosliny, "Id", "Id", zaleznosc.RoslinaGlownaId);
            ViewData["RoslinaTowarzyszacaId"] = new SelectList(_context.Rosliny, "Id", "Id", zaleznosc.RoslinaTowarzyszacaId);
            return View(zaleznosc);
        }

        // GET: Zaleznosc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaleznosc = await _context.Zaleznosci.FindAsync(id);
            if (zaleznosc == null)
            {
                return NotFound();
            }
            ViewData["RoslinaGlownaId"] = new SelectList(_context.Rosliny, "Id", "Id", zaleznosc.RoslinaGlownaId);
            ViewData["RoslinaTowarzyszacaId"] = new SelectList(_context.Rosliny, "Id", "Id", zaleznosc.RoslinaTowarzyszacaId);
            return View(zaleznosc);
        }

        // POST: Zaleznosc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoslinaGlownaId,RoslinaTowarzyszacaId,TypZaleznosci,Opis")] Zaleznosc zaleznosc)
        {
            if (id != zaleznosc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zaleznosc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZaleznoscExists(zaleznosc.Id))
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
            ViewData["RoslinaGlownaId"] = new SelectList(_context.Rosliny, "Id", "Id", zaleznosc.RoslinaGlownaId);
            ViewData["RoslinaTowarzyszacaId"] = new SelectList(_context.Rosliny, "Id", "Id", zaleznosc.RoslinaTowarzyszacaId);
            return View(zaleznosc);
        }

        // GET: Zaleznosc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaleznosc = await _context.Zaleznosci
                .Include(z => z.RoslinaGlowna)
                .Include(z => z.RoslinaTowarzyszaca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaleznosc == null)
            {
                return NotFound();
            }

            return View(zaleznosc);
        }

        // POST: Zaleznosc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zaleznosc = await _context.Zaleznosci.FindAsync(id);
            if (zaleznosc != null)
            {
                _context.Zaleznosci.Remove(zaleznosc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZaleznoscExists(int id)
        {
            return _context.Zaleznosci.Any(e => e.Id == id);
        }
    }
}
