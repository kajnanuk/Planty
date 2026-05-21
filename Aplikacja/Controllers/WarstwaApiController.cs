using Aplikacja.Data;
using Aplikacja.Filters;
using Aplikacja.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aplikacja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuth] // <-- Nasz nowy filtr zabezpieczający dostęp!
    public class WarstwaApiController : ControllerBase
    {
        private readonly AplikacjaContext _context;

        public WarstwaApiController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: api/WarstwaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarstwaDTO>>> GetWarstwy()
        {
            return await _context.Warstwy
                .Select(w => new WarstwaDTO { Id = w.Id, Nazwa = w.Nazwa, Opis = w.Opis })
                .ToListAsync();
        }

        // GET: api/WarstwaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WarstwaDTO>> GetWarstwa(int id)
        {
            var warstwa = await _context.Warstwy.FindAsync(id);

            if (warstwa == null)
            {
                return NotFound();
            }

            return new WarstwaDTO { Id = warstwa.Id, Nazwa = warstwa.Nazwa, Opis = warstwa.Opis };
        }

        // POST: api/WarstwaApi
        [HttpPost]
        public async Task<ActionResult<WarstwaDTO>> PostWarstwa(WarstwaDTO warstwaDTO)
        {
            var warstwa = new Warstwa
            {
                Nazwa = warstwaDTO.Nazwa,
                Opis = warstwaDTO.Opis
            };

            _context.Warstwy.Add(warstwa);
            await _context.SaveChangesAsync();

            warstwaDTO.Id = warstwa.Id;
            return CreatedAtAction(nameof(GetWarstwa), new { id = warstwa.Id }, warstwaDTO);
        }

        // PUT: api/WarstwaApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarstwa(int id, WarstwaDTO warstwaDTO)
        {
            if (id != warstwaDTO.Id)
            {
                return BadRequest();
            }

            var warstwa = await _context.Warstwy.FindAsync(id);
            if (warstwa == null)
            {
                return NotFound();
            }

            warstwa.Nazwa = warstwaDTO.Nazwa;
            warstwa.Opis = warstwaDTO.Opis;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/WarstwaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarstwa(int id)
        {
            var warstwa = await _context.Warstwy.FindAsync(id);
            if (warstwa == null)
            {
                return NotFound();
            }

            // Zapobieganie usunięciu warstwy, jeśli ma przypisane rośliny
            var maRosliny = await _context.Rosliny.AnyAsync(r => r.WarstwaId == id);
            if (maRosliny)
            {
                return BadRequest("Nie można usunąć warstwy, która zawiera rośliny.");
            }

            _context.Warstwy.Remove(warstwa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}