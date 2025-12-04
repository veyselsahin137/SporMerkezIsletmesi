using System.ComponentModel.DataAnnotations;

namespace SporMerkeziIsletmesi.Models
{
    public class Antrenor
    {
        public int AntrenorID { get; set; }   // PK

        [Required]
        [MaxLength(50)]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Uzmanlık Alanı")]
        public string UzmanlikAlani { get; set; }   // Örn: Fitness, Yoga, Pilates

        [Phone]
        [Display(Name = "Telefon")]
        public string? Telefon { get; set; }

        [EmailAddress]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool AktifMi { get; set; } = true;
    }
}
