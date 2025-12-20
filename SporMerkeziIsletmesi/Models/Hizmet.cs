using System.ComponentModel.DataAnnotations;

namespace SporMerkeziIsletmesi.Models
{
    public class Hizmet
    {
        public int HizmetID { get; set; }   // PK

        [Required]
        [MaxLength(100)]
        [Display(Name = "Hizmet Adı")]
        public string HizmetAdi { get; set; }

        [Display(Name = "Süre (dakika)")]
        [Range(10, 300, ErrorMessage = "Süre 10-300 dakika arasında olmalıdır.")]
        public int SureDakika { get; set; }

        [Display(Name = "Ücret (₺)")]
        [Range(0, 10000, ErrorMessage = "Ücret 0-10000 arası olmalıdır.")]
        public decimal Ucret { get; set; }

        //  Hangi salonda veriliyor? (FK)
        [Display(Name = "Salon")]
        public int SalonID { get; set; }

        public Salon? Salon { get; set; }   // Navigation property
    }
}
