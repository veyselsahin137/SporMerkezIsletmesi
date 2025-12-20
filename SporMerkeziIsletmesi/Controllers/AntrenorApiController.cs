using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AntrenorApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AntrenorApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AntrenorApi
        // Tüm antrenörleri JSON olarak döndürür
        [HttpGet]
        [AllowAnonymous]   
        public async Task<IActionResult> GetAntrenorler()
        {
            var antrenorler = await _context.Antrenorler
                .Select(a => new
                {
                    a.AntrenorID,
                    a.Ad,
                    a.Soyad,
                    AdSoyad = a.Ad + " " + a.Soyad,
                    a.UzmanlikAlani,
                    a.Telefon,
                    a.Email,
                    a.AktifMi
                })
                .ToListAsync();

            return Ok(antrenorler);
        }

        // GET: api/AntrenorApi/ByHizmet/3
        // Belirli bir hizmeti verebilen AKTİF antrenörleri LINQ ile filtreler
        [HttpGet("ByHizmet/{hizmetId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAntrenorlerByHizmet(int hizmetId)
        {
            var sonuc = await _context.AntrenorHizmetler
                .Include(ah => ah.Antrenor)
                .Include(ah => ah.Hizmet)
                .Where(ah => ah.HizmetID == hizmetId && ah.Antrenor.AktifMi)
                .Select(ah => new
                {
                    ah.Antrenor.AntrenorID,
                    Ad = ah.Antrenor.Ad,
                    Soyad = ah.Antrenor.Soyad,
                    AdSoyad = ah.Antrenor.Ad + " " + ah.Antrenor.Soyad,
                    UzmanlikAlani = ah.Antrenor.UzmanlikAlani,

                    HizmetID = ah.HizmetID,
                    HizmetAdi = ah.Hizmet.HizmetAdi,
                    SureDakika = ah.Hizmet.SureDakika,
                    Ucret = ah.Hizmet.Ucret
                })
                .ToListAsync();

            return Ok(sonuc);
        }
    }
}
