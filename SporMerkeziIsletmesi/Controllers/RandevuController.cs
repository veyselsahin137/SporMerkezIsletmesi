using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public RandevuController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ ÜYENİN SADECE KENDİ RANDEVULARI
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var uye = await _context.Uyeler.FirstOrDefaultAsync(u => u.KullaniciId == userId);
            if (uye == null)
                return Unauthorized();

            var randevular = await _context.Randevu
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Where(r => r.UyeId == uye.Id)
                .OrderByDescending(r => r.Tarih)
                .ToListAsync();

            return View(randevular);
        }

        // ✅ CREATE (GET)
        public IActionResult Create()
        {
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi");
            ViewBag.Antrenorler = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }

        // ✅ CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu)
        {
            var userId = _userManager.GetUserId(User);
            var uye = await _context.Uyeler.FirstOrDefaultAsync(u => u.KullaniciId == userId);

            if (uye == null)
                return Unauthorized();

            // 🔒 Üye otomatik bağlanır
            randevu.UyeId = uye.Id;
            randevu.Durum = "Beklemede";

            // ❌ ÇAKIŞMA KONTROLÜ
            bool cakismaVarMi = await _context.Randevu.AnyAsync(r =>
                r.AntrenorId == randevu.AntrenorId &&
                r.Tarih == randevu.Tarih
            );

            if (cakismaVarMi)
            {
                ModelState.AddModelError("", "Bu tarih ve saatte antrenörün başka bir randevusu var.");
            }

            if (ModelState.IsValid)
            {
                _context.Randevu.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", randevu.HizmetId);

            // 🔁 Hizmete göre tekrar antrenör doldur
            var antrenorler = _context.AntrenorHizmetler
                .Where(x => x.HizmetID == randevu.HizmetId)
                .Select(x => x.Antrenor)
                .Distinct()
                .ToList();

            ViewBag.Antrenorler = new SelectList(antrenorler, "AntrenorID", "Ad", randevu.AntrenorId);

            return View(randevu);
        }

        // ✅ AJAX – HİZMETE GÖRE ANTRENÖR GETİR
        public JsonResult HizmeteGoreAntrenorGetir(int hizmetId)
        {
            var antrenorler = _context.AntrenorHizmetler
                .Where(x => x.HizmetID == hizmetId)
                .Select(x => new
                {
                    x.Antrenor.AntrenorID,
                    x.Antrenor.Ad
                })
                .Distinct()
                .ToList();

            return Json(antrenorler);
        }
    }
}
