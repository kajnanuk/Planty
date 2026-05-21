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
    public class OgrodUzytkownikaController : Controller
    {
        private readonly AplikacjaContext _context;

        public OgrodUzytkownikaController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: OgrodUzytkownika
        public async Task<IActionResult> Index()
        {
            var aplikacjaContext = _context.OgrodyUzytkownikow.Include(o => o.Roslina).Include(o => o.Uzytkownik);
            return View(await aplikacjaContext.ToListAsync());
        }

        // GET: OgrodUzytkownika/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrodUzytkownika = await _context.OgrodyUzytkownikow
                .Include(o => o.Roslina)
                .Include(o => o.Uzytkownik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ogrodUzytkownika == null)
            {
                return NotFound();
            }

            return View(ogrodUzytkownika);
        }

        // GET: OgrodUzytkownika/Create
        public IActionResult Create()
        {
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id");
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id");
            return View();
        }

        // POST: OgrodUzytkownika/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UzytkownikId,RoslinaId,DataPosadzenia,IloscSztuk,WlasneNotatki")] OgrodUzytkownika ogrodUzytkownika)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ogrodUzytkownika);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id", ogrodUzytkownika.RoslinaId);
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id", ogrodUzytkownika.UzytkownikId);
            return View(ogrodUzytkownika);
        }

        // GET: OgrodUzytkownika/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrodUzytkownika = await _context.OgrodyUzytkownikow.FindAsync(id);
            if (ogrodUzytkownika == null)
            {
                return NotFound();
            }
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id", ogrodUzytkownika.RoslinaId);
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id", ogrodUzytkownika.UzytkownikId);
            return View(ogrodUzytkownika);
        }

        // POST: OgrodUzytkownika/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UzytkownikId,RoslinaId,DataPosadzenia,IloscSztuk,WlasneNotatki")] OgrodUzytkownika ogrodUzytkownika)
        {
            if (id != ogrodUzytkownika.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ogrodUzytkownika);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OgrodUzytkownikaExists(ogrodUzytkownika.Id))
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
            ViewData["RoslinaId"] = new SelectList(_context.Rosliny, "Id", "Id", ogrodUzytkownika.RoslinaId);
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id", ogrodUzytkownika.UzytkownikId);
            return View(ogrodUzytkownika);
        }

        // GET: OgrodUzytkownika/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrodUzytkownika = await _context.OgrodyUzytkownikow
                .Include(o => o.Roslina)
                .Include(o => o.Uzytkownik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ogrodUzytkownika == null)
            {
                return NotFound();
            }

            return View(ogrodUzytkownika);
        }

        // POST: OgrodUzytkownika/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ogrodUzytkownika = await _context.OgrodyUzytkownikow.FindAsync(id);
            if (ogrodUzytkownika != null)
            {
                _context.OgrodyUzytkownikow.Remove(ogrodUzytkownika);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OgrodUzytkownikaExists(int id)
        {
            return _context.OgrodyUzytkownikow.Any(e => e.Id == id);
        }
    }
}
