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
using System.Security.Cryptography;
using System.Text;

namespace Aplikacja.Controllers
{
    [AdminTylko]
    public class UzytkownikController : Controller
    {
        private readonly AplikacjaContext _context;

        public UzytkownikController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: Uzytkownik
        public async Task<IActionResult> Index()
        {
            return View(await _context.Uzytkownicy.ToListAsync());
        }

        // GET: Uzytkownik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownicy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            return View(uzytkownik);
        }

        // GET: Uzytkownik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uzytkownik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NazwaUzytkownika,HasloHash,Rola")] Uzytkownik uzytkownik)
        {
            if (ModelState.IsValid)
            {
                // zahashowanie hasla
                uzytkownik.HasloHash = ComputeSha256Hash(uzytkownik.HasloHash);

                // generowanie tokenu API
                uzytkownik.ApiToken = Guid.NewGuid().ToString();

                _context.Add(uzytkownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uzytkownik);
        }
        // Metoda pomocnicza do tworzenia hashu
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // GET: Uzytkownik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownicy.FindAsync(id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            return View(uzytkownik);
        }

        // POST: Uzytkownik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NazwaUzytkownika,HasloHash,Rola,ApiToken")] Uzytkownik uzytkownik)
        {
            if (id != uzytkownik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uzytkownik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UzytkownikExists(uzytkownik.Id))
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
            return View(uzytkownik);
        }

        // GET: Uzytkownik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownicy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            return View(uzytkownik);
        }

        // POST: Uzytkownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Uzytkownicy == null)
            {
                return Problem("Entity set 'AplikacjaContext.Uzytkownicy' is null.");
            }

            var uzytkownik = await _context.Uzytkownicy.FindAsync(id);
            
            if (uzytkownik != null)
            {
                // znajdujemy wszystkie rekordy nalezace do danego uzytkownika
                var rekordyOgrodu = _context.OgrodyUzytkownikow.Where(o => o.UzytkownikId == id);
                
                // usuwamy je z bazy
                if (rekordyOgrodu.Any())
                {
                    _context.OgrodyUzytkownikow.RemoveRange(rekordyOgrodu);
                }

                // teraz usuwamy samego uzytkownika
                _context.Uzytkownicy.Remove(uzytkownik);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UzytkownikExists(int id)
        {
            return _context.Uzytkownicy.Any(e => e.Id == id);
        }
    }
}
