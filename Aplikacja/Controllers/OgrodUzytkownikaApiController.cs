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
    public class OgrodUzytkownikaApiController : ControllerBase
    {
        private readonly AplikacjaContext _context;

        public OgrodUzytkownikaApiController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: api/OgrodUzytkownikaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OgrodUzytkownikaDTO>>> GetOgrody()
        {
            return await _context.OgrodyUzytkownikow
                .Select(o => new OgrodUzytkownikaDTO
                {
                    Id = o.Id,
                    UzytkownikId = o.UzytkownikId,
                    RoslinaId = o.RoslinaId,
                    DataPosadzenia = o.DataPosadzenia,
                    IloscSztuk = o.IloscSztuk,
                    WlasneNotatki = o.WlasneNotatki
                })
                .ToListAsync();
        }

        // GET: api/OgrodUzytkownikaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OgrodUzytkownikaDTO>> GetOgrod(int id)
        {
            var o = await _context.OgrodyUzytkownikow.FindAsync(id);
            if (o == null) return NotFound();

            return new OgrodUzytkownikaDTO
            {
                Id = o.Id,
                UzytkownikId = o.UzytkownikId,
                RoslinaId = o.RoslinaId,
                DataPosadzenia = o.DataPosadzenia,
                IloscSztuk = o.IloscSztuk,
                WlasneNotatki = o.WlasneNotatki
            };
        }

        // POST: api/OgrodUzytkownikaApi
        [HttpPost]
        public async Task<ActionResult<OgrodUzytkownikaDTO>> PostOgrod(OgrodUzytkownikaDTO dto)
        {
            var ogrod = new OgrodUzytkownika
            {
                UzytkownikId = dto.UzytkownikId,
                RoslinaId = dto.RoslinaId,
                DataPosadzenia = dto.DataPosadzenia,
                IloscSztuk = dto.IloscSztuk,
                WlasneNotatki = dto.WlasneNotatki
            };

            _context.OgrodyUzytkownikow.Add(ogrod);
            await _context.SaveChangesAsync();

            dto.Id = ogrod.Id;
            return CreatedAtAction(nameof(GetOgrod), new { id = ogrod.Id }, dto);
        }

        // PUT: api/OgrodUzytkownikaApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOgrod(int id, OgrodUzytkownikaDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            var ogrod = await _context.OgrodyUzytkownikow.FindAsync(id);
            if (ogrod == null) return NotFound();

            ogrod.UzytkownikId = dto.UzytkownikId;
            ogrod.RoslinaId = dto.RoslinaId;
            ogrod.DataPosadzenia = dto.DataPosadzenia;
            ogrod.IloscSztuk = dto.IloscSztuk;
            ogrod.WlasneNotatki = dto.WlasneNotatki;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/OgrodUzytkownikaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOgrod(int id)
        {
            var ogrod = await _context.OgrodyUzytkownikow.FindAsync(id);
            if (ogrod == null) return NotFound();

            _context.OgrodyUzytkownikow.Remove(ogrod);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}