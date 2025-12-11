using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize] // Randevu işlemleri için giriş zorunlu
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RandevuController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================================================
        //  1) ÜYE SADECE KENDİ RANDEVULARINI GÖRSÜN
        // =========================================================
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.KullaniciId == user.Id);

            if (uye == null)
            {
                // Kullanıcının Uye kaydı yoksa önce profilini tamamlasın
                return RedirectToAction("Profilim", "Uye");
            }

            var randevular = await _context.Randevu
                .Where(r => r.UyeId == uye.Id)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .ToListAsync();

            return View(randevular);
        }

        // =========================================================
        //  2) RANDEVU OLUŞTURMA SAYFASI (GET)
        //     - UyeId otomatik atanacak (dropdown yok)
        //     - Hizmete göre antrenörler JS ile filtrelenecek
        //     - Saatler antrenörün müsaitlik tablosuna göre dolacak
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.KullaniciId == user.Id);

            if (uye == null)
                return RedirectToAction("Profilim", "Uye");

            // ÜyeId formda gizli alanla gidecek
            ViewData["UyeId"] = uye.Id;

            // Hizmet dropdown’ı doldur
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi");

            // Antrenör dropdown’ı ilk açılışta boş, JS ile dolacak
            ViewData["AntrenorId"] = new SelectList(Enumerable.Empty<SelectListItem>());

            return View();
        }

        // =========================================================
        //  3) RANDEVU OLUŞTURMA (POST)
        //     - ÜyeId: giriş yapan kullanıcıdan
        //     - Tarih: seciliTarih + seciliSaat birleştirilerek
        //     - Durum: "Beklemede"
        //     - ÇAKIŞMA: aynı tarih/saatte aynı antrenöre randevu var mı?
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu, string seciliTarih, string seciliSaat)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.KullaniciId == user.Id);

            if (uye == null)
                return RedirectToAction("Profilim", "Uye");

            // ÜyeId’yi zorla bağla (formdaki değeri dikkate alma)
            randevu.UyeId = uye.Id;

            // Tarih + saat birleştir
            if (!string.IsNullOrWhiteSpace(seciliTarih) && !string.IsNullOrWhiteSpace(seciliSaat))
            {
                if (DateTime.TryParse($"{seciliTarih} {seciliSaat}", out var dt))
                {
                    randevu.Tarih = dt;
                }
                else
                {
                    ModelState.AddModelError("Tarih", "Seçilen tarih veya saat geçersiz.");
                }
            }
            else
            {
                ModelState.AddModelError("Tarih", "Lütfen tarih ve saat seçiniz.");
            }

            // Varsayılan durum
            randevu.Durum = "Beklemede";

            // Aynı anda aynı antrenöre daha önce randevu var mı?
            bool cakisma = await _context.Randevu.AnyAsync(r =>
                r.AntrenorId == randevu.AntrenorId &&
                r.Tarih == randevu.Tarih);

            if (cakisma)
            {
                ModelState.AddModelError(string.Empty, "Bu tarih ve saatte antrenörün başka bir randevusu var.");
            }

            if (ModelState.IsValid)
            {
                _context.Randevu.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata durumunda dropdown'ları tekrar doldur
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", randevu.HizmetId);
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", randevu.AntrenorId);

            return View(randevu);
        }

        // =========================================================
        //  4) HİZMETE GÖRE ANTRENÖR FİLTRELEME (AJAX)
        // =========================================================
        [HttpGet]
        public async Task<JsonResult> GetAntrenorByHizmet(int hizmetId)
        {
            var liste = await _context.AntrenorHizmetler
                .Where(x => x.HizmetID == hizmetId)
                .Select(x => new
                {
                    id = x.AntrenorID,
                    ad = x.Antrenor.Ad
                })
                .Distinct()
                .ToListAsync();

            return Json(liste);
        }

        // =========================================================
        //  5) ANTRENÖRÜN MÜSAİT SAATLERİ (AJAX)
        //     - AntrenorMusaitlik tablosundan saatleri üret
        //     - Randevu tablosundaki dolu saatleri çıkar
        // =========================================================
        [HttpGet]
        public async Task<JsonResult> GetAvailableHours(int antrenorId, string tarih)
        {
            if (!DateTime.TryParse(tarih, out var seciliTarih))
                return Json(Array.Empty<string>());

            var gun = seciliTarih.DayOfWeek.ToString(); // Monday, Tuesday ...

            var musait = await _context.AntrenorMusaitlikler
                .FirstOrDefaultAsync(m => m.AntrenorID == antrenorId && m.Gun == gun);

            if (musait == null)
                return Json(Array.Empty<string>());

            // Saatleri 1 saatlik aralıklarla üret
            var saatler = new List<string>();
            var basla = musait.BaslangicSaati;
            var bitis = musait.BitisSaati;

            while (basla < bitis)
            {
                saatler.Add(basla.ToString(@"hh\:mm"));
                basla = basla.Add(TimeSpan.FromHours(1));
            }

            // Aynı gün o antrenöre alınmış randevuların saatlerini bul
            var doluSaatler = await _context.Randevu
                .Where(r => r.AntrenorId == antrenorId && r.Tarih.Date == seciliTarih.Date)
                .Select(r => r.Tarih.TimeOfDay)
                .ToListAsync();

            // Dolu saatleri listeden çıkar
            var uygunSaatler = saatler
                .Where(s => !doluSaatler.Contains(TimeSpan.Parse(s)))
                .ToList();

            return Json(uygunSaatler);
        }
    }
}
