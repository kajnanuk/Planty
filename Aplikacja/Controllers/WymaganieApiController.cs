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
    public class WymaganieApiController : ControllerBase
    {
        private readonly AplikacjaContext _context;

        public WymaganieApiController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: api/WymaganieApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WymaganieDTO>>> GetWymagania()
        {
            return await _context.Wymagania
                .Select(w => new WymaganieDTO
                {
                    Id = w.Id,
                    RoslinaId = w.RoslinaId,
                    Naslonecznienie = w.Naslonecznienie,
                    RodzajGleby = w.RodzajGleby,
                    ZapotrzebowanieNaWode = w.ZapotrzebowanieNaWode,
                    Mrozoodpornosc = w.Mrozoodpornosc
                })
                .ToListAsync();
        }

        // GET: api/WymaganieApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WymaganieDTO>> GetWymaganie(int id)
        {
            var w = await _context.Wymagania.FindAsync(id);
            if (w == null) return NotFound();

            return new WymaganieDTO
            {
                Id = w.Id,
                RoslinaId = w.RoslinaId,
                Naslonecznienie = w.Naslonecznienie,
                RodzajGleby = w.RodzajGleby,
                ZapotrzebowanieNaWode = w.ZapotrzebowanieNaWode,
                Mrozoodpornosc = w.Mrozoodpornosc
            };
        }

        // POST: api/WymaganieApi
        [HttpPost]
        public async Task<ActionResult<WymaganieDTO>> PostWymaganie(WymaganieDTO dto)
        {
            var wymaganie = new Wymaganie
            {
                RoslinaId = dto.RoslinaId,
                Naslonecznienie = dto.Naslonecznienie,
                RodzajGleby = dto.RodzajGleby,
                ZapotrzebowanieNaWode = dto.ZapotrzebowanieNaWode,
                Mrozoodpornosc = dto.Mrozoodpornosc
            };

            _context.Wymagania.Add(wymaganie);
            await _context.SaveChangesAsync();

            dto.Id = wymaganie.Id;
            return CreatedAtAction(nameof(GetWymaganie), new { id = wymaganie.Id }, dto);
        }

        // PUT: api/WymaganieApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWymaganie(int id, WymaganieDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            var wymaganie = await _context.Wymagania.FindAsync(id);
            if (wymaganie == null) return NotFound();

            wymaganie.RoslinaId = dto.RoslinaId;
            wymaganie.Naslonecznienie = dto.Naslonecznienie;
            wymaganie.RodzajGleby = dto.RodzajGleby;
            wymaganie.ZapotrzebowanieNaWode = dto.ZapotrzebowanieNaWode;
            wymaganie.Mrozoodpornosc = dto.Mrozoodpornosc;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/WymaganieApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWymaganie(int id)
        {
            var wymaganie = await _context.Wymagania.FindAsync(id);
            if (wymaganie == null) return NotFound();

            _context.Wymagania.Remove(wymaganie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}