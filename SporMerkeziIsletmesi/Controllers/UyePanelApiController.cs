using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;
using SporMerkeziIsletmesi.Models.ViewModels;
using SporMerkeziIsletmesi.Helpers;



namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UyePanelApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UyePanelApiController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===============================
        // RANDEVULARIM
        // ===============================
        [HttpGet("randevularim")]
        public async Task<IActionResult> Randevularim()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var uye = await _context.Uyeler
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdentityUserId == user.Id);

            if (uye == null)
                return Ok(new { items = new object[0] });

            var items = await _context.Randevular
                .AsNoTracking()
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Where(r => r.UyeId == uye.Id)
                .OrderByDescending(r => r.Tarih)
                .Select(r => new
                {
                    id = r.Id,
                    tarih = r.Tarih,
                    baslangic = r.BaslangicSaati,
                    bitis = r.BitisSaati,
                    antrenor = r.Antrenor != null ? r.Antrenor.Ad + " " + r.Antrenor.Soyad : "",
                    hizmet = r.Hizmet != null ? r.Hizmet.HizmetAdi : "",
                    durum = DurumText(r.Durum)
                })
                .ToListAsync();

            return Ok(new { items });
        }

        // ===============================
        // HİZMET → ANTRENÖR
        // ===============================
        [HttpGet("hizmet/{hizmetId}/antrenorler")]
        public IActionResult HizmeteGoreAntrenorler(int hizmetId)
        {
            var sonuc = _context.AntrenorHizmetler
                .Include(x => x.Antrenor)
                .Include(x => x.Hizmet)
                    .ThenInclude(h => h.Salon)
                .Where(x => x.HizmetID == hizmetId && x.Antrenor.AktifMi)
                .Select(x => new
                {
                    antrenorId = x.Antrenor.AntrenorID,
                    adSoyad = x.Antrenor.Ad + " " + x.Antrenor.Soyad,
                    salonAdi = x.Hizmet.Salon.SalonAdi,
                    salonAdres = x.Hizmet.Salon.Adres,
                    acilis = x.Hizmet.Salon.AcilisSaati,
                    kapanis = x.Hizmet.Salon.KapanisSaati
                })
                .ToList();

            return Ok(sonuc);
        }

        // ===============================
        // ANTRENÖR → BOŞ SLOT
        // ===============================
        [HttpGet("antrenor/{antrenorId}/bos-slotlar")]
        public async Task<IActionResult> BosSlotlar(int antrenorId, int hizmetId)
        {
            var hizmet = await _context.Hizmetler.FindAsync(hizmetId);
            if (hizmet == null)
                return BadRequest();

            int sureDakika = hizmet.SureDakika;

            var musaitlikler = await _context.AntrenorMusaitlikler
                .Where(m => m.AntrenorID == antrenorId)
                .ToListAsync();

            var randevular = await _context.Randevular
                .Where(r => r.AntrenorID == antrenorId)
                .ToListAsync();

            var slots = new List<RandevuSlotVM>();

            foreach (var m in musaitlikler)
            {
                DateTime tarih = DateHelper.GunToDate(m.Gun);

                for (var saat = m.BaslangicSaati;
                     saat.Add(TimeSpan.FromMinutes(sureDakika)) <= m.BitisSaati;
                     saat = saat.Add(TimeSpan.FromMinutes(sureDakika)))
                {
                    var bitis = saat.Add(TimeSpan.FromMinutes(sureDakika));

                    bool cakisma = randevular.Any(r =>
                        r.Tarih.Date == tarih.Date &&
                        saat < r.BitisSaati &&
                        bitis > r.BaslangicSaati
                    );

                    if (!cakisma)
                    {
                        slots.Add(new RandevuSlotVM
                        {
                            Tarih = tarih,
                            Baslangic = saat,
                            Bitis = bitis,
                            GunYazi = m.Gun
                        });
                    }
                }
            }

            return Ok(slots);
        }

        // ===============================
        // YARDIMCI METOTLAR (API İÇİN)
        // ===============================
        private static string DurumText(RandevuDurum durum)
        {
            return durum switch
            {
                RandevuDurum.Beklemede => "Beklemede",
                RandevuDurum.Onaylandi => "Onaylandı",
                RandevuDurum.IptalEdildi => "İptal Edildi",
                _ => "Bilinmiyor"
            };
        }

        private static string GunCevir(DateTime tarih)
        {
            return tarih.DayOfWeek switch
            {
                DayOfWeek.Monday => "Pazartesi",
                DayOfWeek.Tuesday => "Salı",
                DayOfWeek.Wednesday => "Çarşamba",
                DayOfWeek.Thursday => "Perşembe",
                DayOfWeek.Friday => "Cuma",
                DayOfWeek.Saturday => "Cumartesi",
                DayOfWeek.Sunday => "Pazar",
                _ => ""
            };
        }

        private static DateTime GunToDate(string gun)
        {
            var today = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                var d = today.AddDays(i);
                if (GunCevir(d) == gun)
                    return d;
            }

            throw new Exception("Geçerli gün bulunamadı");
        }
    }
}
