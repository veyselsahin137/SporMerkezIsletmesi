using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize] // Giriş yapan kullanıcı zorunlu
    public class UyeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UyeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------
        //   PROFİLİM  (Üyenin kendi profilini görmesi)
        // ------------------------------------------------------
        public async Task<IActionResult> Profilim()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var uye = await _context.Uyeler
                .Include(u => u.Salon)
                .FirstOrDefaultAsync(u => u.KullaniciId == userId);

            if (uye == null)
                return NotFound();

            return View("Details", uye);
        }

        // ------------------------------------------------------
        //   PROFİLİ DÜZENLE (Sadece kendini düzenleyebilir)
        // ------------------------------------------------------
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.KullaniciId == userId);

            if (uye == null)
                return NotFound();

            ViewData["SalonId"] = new SelectList(_context.Salonlar, "SalonID", "SalonAdi", uye.SalonId);
            return View(uye);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,KullaniciId,SalonId,Yas,Telefon,Boy,Kilo,Hedef,AktiviteSeviyesi,YagOrani,TercihEdilenZaman")] Uye form)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(u => u.Id == form.Id && u.KullaniciId == userId);

            if (uye == null)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                ViewData["SalonId"] = new SelectList(_context.Salonlar, "SalonID", "SalonAdi", form.SalonId);
                return View(form);
            }

            // Güncelleme
            uye.SalonId = form.SalonId;
            uye.Yas = form.Yas;
            uye.Telefon = form.Telefon;
            uye.Boy = form.Boy;
            uye.Kilo = form.Kilo;
            uye.Hedef = form.Hedef;
            uye.AktiviteSeviyesi = form.AktiviteSeviyesi;
            uye.YagOrani = form.YagOrani;
            uye.TercihEdilenZaman = form.TercihEdilenZaman;

            _context.Update(uye);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Profilim));
        }

        // ------------------------------------------------------
        //   Listeleme kapalı — sadece Profilim ekranı kullanılacak
        // ------------------------------------------------------
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Profilim));
        }

        // ------------------------------------------------------
        //   Create / Delete devre dışı (Identity zaten oluşturuyor)
        // ------------------------------------------------------
        public IActionResult Delete() => Unauthorized();
    }
}
