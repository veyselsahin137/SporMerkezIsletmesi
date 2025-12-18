using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize] // sadece giriş yapan üyeler
    public class UyePanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UyePanelController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ================================
        // ÜYE PANELİ ANA SAYFA
        // ================================
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.IdentityUserId == user.Id);

            if (uye == null)
                return RedirectToAction("Create", "Uye");

            return View(uye);
        }

        // ================================
        // ÜYENİN RANDEVULARI
        // ================================
        public async Task<IActionResult> Randevularim()
        {
            var user = await _userManager.GetUserAsync(User);

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.IdentityUserId == user.Id);

            if (uye == null)
                return RedirectToAction("Index");

            var randevular = await _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Durum)
                .Where(r => r.UyeId == uye.Id)
                .OrderByDescending(r => r.Tarih)
                .ToListAsync();

            return View(randevular);
        }

        // ================================
        // YENİ RANDEVU TALEBİ (GET)
        // ================================
        public IActionResult RandevuOlustur()
        {
            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();

            return View();
        }

        // ================================
        // YENİ RANDEVU TALEBİ (POST)
        // ================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RandevuOlustur(Randevu randevu)
        {
            var user = await _userManager.GetUserAsync(User);

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.IdentityUserId == user.Id);

            if (uye == null)
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                randevu.UyeId = uye.Id;
                randevu.Durum = 0; // BEKLEMEDE
                randevu.OlusturmaTarihi = DateTime.Now;

                _context.Randevular.Add(randevu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Randevularim));
            }

            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();

            return View(randevu);
        }
    }
}
