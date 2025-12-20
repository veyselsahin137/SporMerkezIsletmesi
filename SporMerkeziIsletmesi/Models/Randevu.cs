using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporMerkeziIsletmesi.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }

        
        // ÜYE
        
        [Required]
        [Display(Name = "Üye")]
        public int UyeId { get; set; }

        [ForeignKey(nameof(UyeId))]
        public Uye Uye { get; set; } = null!;

        
        // ANTRENÖR
        
        [Required]
        [Display(Name = "Antrenör")]
        public int AntrenorID { get; set; }

        [ForeignKey(nameof(AntrenorID))]
        public Antrenor Antrenor { get; set; } = null!;

        
        // HİZMET
        
        [Required]
        [Display(Name = "Hizmet")]
        public int HizmetID { get; set; }

        [ForeignKey(nameof(HizmetID))]
        public Hizmet Hizmet { get; set; } = null!;

        
        // TARİH & SAAT
        
        [Required]
        [Display(Name = "Randevu Tarihi")]
        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        [Required]
        [Display(Name = "Başlangıç Saati")]
        public TimeSpan BaslangicSaati { get; set; }

        [Required]
        [Display(Name = "Bitiş Saati")]
        public TimeSpan BitisSaati { get; set; }

        
        // DURUM
       
        [Required]
        [Display(Name = "Durum")]
        public RandevuDurum Durum { get; set; } = RandevuDurum.Beklemede;

        
        // METADATA
        
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
}
