using System.ComponentModel.DataAnnotations;

namespace SporMerkeziIsletmesi.Models
{
    public class YapayZekaOneriViewModel
    {
        [Required]
        [Display(Name = "Boy (cm)")]
        public int BoyCm { get; set; }

        [Required]
        [Display(Name = "Kilo (kg)")]
        public int KiloKg { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Hedef")]
        public string Hedef { get; set; } = string.Empty;
        // Örnek: Kilo vermek, kas yapmak, formu korumak

        [MaxLength(500)]
        [Display(Name = "Ek Bilgi")]
        public string? EkBilgi { get; set; }
        // Örnek: Diz sakatlığım var, koşu yapamıyorum gibi…

        [Display(Name = "Yapay Zekâ Önerisi")]
        public string? YapayZekaCevap { get; set; }
        // OpenAI'den gelen metni burada göstereceğiz
    }
}
