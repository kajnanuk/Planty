using Aplikacja.Data;
using Aplikacja.Filters;
using Aplikacja.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aplikacja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuth] // zabezpieczenie tokenem
    public class RoslinaApiController : ControllerBase
    {
        private readonly AplikacjaContext _context;

        public RoslinaApiController(AplikacjaContext context)
        {
            _context = context;
        }

        // GET: api/RoslinaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoslinaDTO>>> GetRosliny()
        {
            return await _context.Rosliny
                .Select(r => new RoslinaDTO 
                { 
                    Id = r.Id, 
                    NazwaPolska = r.NazwaPolska, 
                    NazwaLacinska = r.NazwaLacinska, 
                    Opis = r.Opis, 
                    WarstwaId = r.WarstwaId 
                })
                .ToListAsync();
        }

        // GET: api/RoslinaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoslinaDTO>> GetRoslina(int id)
        {
            var r = await _context.Rosliny.FindAsync(id);
            if (r == null) return NotFound();

            return new RoslinaDTO 
            { 
                Id = r.Id, 
                NazwaPolska = r.NazwaPolska, 
                NazwaLacinska = r.NazwaLacinska, 
                Opis = r.Opis, 
                WarstwaId = r.WarstwaId 
            };
        }

        // POST: api/RoslinaApi
        [HttpPost]
        public async Task<ActionResult<RoslinaDTO>> PostRoslina(RoslinaDTO roslinaDTO)
        {
            var roslina = new Roslina
            {
                NazwaPolska = roslinaDTO.NazwaPolska,
                NazwaLacinska = roslinaDTO.NazwaLacinska,
                Opis = roslinaDTO.Opis,
                WarstwaId = roslinaDTO.WarstwaId
            };

            _context.Rosliny.Add(roslina);
            await _context.SaveChangesAsync();

            roslinaDTO.Id = roslina.Id;
            return CreatedAtAction(nameof(GetRoslina), new { id = roslina.Id }, roslinaDTO);
        }

        // PUT: api/RoslinaApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoslina(int id, RoslinaDTO roslinaDTO)
        {
            if (id != roslinaDTO.Id) return BadRequest();

            var roslina = await _context.Rosliny.FindAsync(id);
            if (roslina == null) return NotFound();

            roslina.NazwaPolska = roslinaDTO.NazwaPolska;
            roslina.NazwaLacinska = roslinaDTO.NazwaLacinska;
            roslina.Opis = roslinaDTO.Opis;
            roslina.WarstwaId = roslinaDTO.WarstwaId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/RoslinaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoslina(int id)
        {
            var roslina = await _context.Rosliny.FindAsync(id);
            if (roslina == null) return NotFound();

            _context.Rosliny.Remove(roslina);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}