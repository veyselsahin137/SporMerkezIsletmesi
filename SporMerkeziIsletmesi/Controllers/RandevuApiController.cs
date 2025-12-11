using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandevuApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RandevuApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/randevu
        [HttpGet]
        public async Task<IActionResult> GetRandevular()
        {
            var list = await _context.Randevu
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Uye)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/randevu/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRandevu(int id)
        {
            var r = await _context.Randevu
                .Include(a => a.Antrenor)
                .Include(h => h.Hizmet)
                .Include(u => u.Uye)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (r == null)
                return NotFound();

            return Ok(r);
        }

        // POST: api/randevu
        [HttpPost]
        public async Task<IActionResult> CreateRandevu([FromBody] Randevu model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Randevu.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRandevu), new { id = model.Id }, model);
        }

        // DELETE: api/randevu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRandevu(int id)
        {
            var r = await _context.Randevu.FindAsync(id);
            if (r == null)
                return NotFound();

            _context.Randevu.Remove(r);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
