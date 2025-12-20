using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RandevuYonetimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevuYonetimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin Randevu Listesi
        public async Task<IActionResult> Index()
        {
            var randevular = await _context.Randevular
                .Include(r => r.Uye)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .OrderByDescending(r => r.OlusturmaTarihi)
                .ToListAsync();

            return View(randevular);
        }

        // POST: Onayla
        [HttpPost]
        public async Task<IActionResult> Onayla(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return NotFound();

            randevu.Durum = RandevuDurum.Onaylandi;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Reddet
        [HttpPost]
        public async Task<IActionResult> Reddet(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return NotFound();

            randevu.Durum = RandevuDurum.IptalEdildi;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
