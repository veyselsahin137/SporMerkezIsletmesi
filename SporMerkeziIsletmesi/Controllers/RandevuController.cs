using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ ÜYE SADECE KENDİ RANDEVULARINI GÖRÜR
        public async Task<IActionResult> Index()
        {
            var kullaniciId = User.Identity!.Name;

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.KullaniciId == kullaniciId);

            if (uye == null)
                return NotFound("Üye bulunamadı");

            var randevular = await _context.Randevu
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Where(r => r.UyeId == uye.Id)
                .OrderBy(r => r.Tarih)
                .ToListAsync();

            return View(randevular);
        }

        // ✅ RANDEVU OLUŞTURMA SAYFASI
        public IActionResult Create()
        {
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad");
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi");
            return View();
        }

        // ✅ RANDEVU KAYDETME
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu)
        {
            var kullaniciId = User.Identity!.Name;

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.KullaniciId == kullaniciId);

            if (uye == null)
                return NotFound("Üye bulunamadı");

            // 🔒 UyeId otomatik bağlanıyor
            randevu.UyeId = uye.Id;
            randevu.Durum = "Beklemede";

            // ⛔ ÇAKIŞMA KONTROLÜ
            bool cakismaVarMi = await _context.Randevu.AnyAsync(r =>
                r.AntrenorId == randevu.AntrenorId &&
                r.Tarih == randevu.Tarih
            );

            if (cakismaVarMi)
            {
                ModelState.AddModelError("", "Bu tarih ve saatte antrenörün başka bir randevusu var.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", randevu.AntrenorId);
                ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", randevu.HizmetId);
                return View(randevu);
            }

            _context.Randevu.Add(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
