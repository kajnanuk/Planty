using Aplikacja.Data;
using Aplikacja.Filters;
using Aplikacja.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aplikacja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuth]
    public class ZaleznoscApiController : ControllerBase
    {
        private readonly AplikacjaContext _context;

        public ZaleznoscApiController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: api/ZaleznoscApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZaleznoscDTO>>> GetZaleznosci()
        {
            return await _context.Zaleznosci
                .Select(z => new ZaleznoscDTO
                {
                    Id = z.Id,
                    RoslinaGlownaId = z.RoslinaGlownaId,
                    RoslinaTowarzyszacaId = z.RoslinaTowarzyszacaId,
                    TypZaleznosci = z.TypZaleznosci,
                    Opis = z.Opis
                })
                .ToListAsync();
        }

        // GET: api/ZaleznoscApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZaleznoscDTO>> GetZaleznosc(int id)
        {
            var z = await _context.Zaleznosci.FindAsync(id);
            if (z == null) return NotFound();

            return new ZaleznoscDTO
            {
                Id = z.Id,
                RoslinaGlownaId = z.RoslinaGlownaId,
                RoslinaTowarzyszacaId = z.RoslinaTowarzyszacaId,
                TypZaleznosci = z.TypZaleznosci,
                Opis = z.Opis
            };
        }

        // POST: api/ZaleznoscApi
        [HttpPost]
        public async Task<ActionResult<ZaleznoscDTO>> PostZaleznosc(ZaleznoscDTO dto)
        {
            var zaleznosc = new Zaleznosc
            {
                RoslinaGlownaId = dto.RoslinaGlownaId,
                RoslinaTowarzyszacaId = dto.RoslinaTowarzyszacaId,
                TypZaleznosci = dto.TypZaleznosci,
                Opis = dto.Opis
            };

            _context.Zaleznosci.Add(zaleznosc);
            await _context.SaveChangesAsync();

            dto.Id = zaleznosc.Id;
            return CreatedAtAction(nameof(GetZaleznosc), new { id = zaleznosc.Id }, dto);
        }

        // PUT: api/ZaleznoscApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZaleznosc(int id, ZaleznoscDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            var zaleznosc = await _context.Zaleznosci.FindAsync(id);
            if (zaleznosc == null) return NotFound();

            zaleznosc.RoslinaGlownaId = dto.RoslinaGlownaId;
            zaleznosc.RoslinaTowarzyszacaId = dto.RoslinaTowarzyszacaId;
            zaleznosc.TypZaleznosci = dto.TypZaleznosci;
            zaleznosc.Opis = dto.Opis;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ZaleznoscApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZaleznosc(int id)
        {
            var zaleznosc = await _context.Zaleznosci.FindAsync(id);
            if (zaleznosc == null) return NotFound();

            _context.Zaleznosci.Remove(zaleznosc);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}