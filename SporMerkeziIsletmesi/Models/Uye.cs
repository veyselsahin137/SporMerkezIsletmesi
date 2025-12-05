using System.ComponentModel.DataAnnotations;

namespace SporMerkeziIsletmesi.Models
{
    public class Uye
    {
        public int Id { get; set; }

        // Identity Kullanıcı bağlantısı
        [Required]
        public string KullaniciId { get; set; }

        // Üyenin bağlı olduğu spor salonu
        [Required]
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        // Kişisel bilgiler
        [Required]
        [Range(1, 120, ErrorMessage = "Yaş 1 ile 120 arasında olmalıdır.")]
        public int Yas { get; set; }

        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string Telefon { get; set; }

        // Fiziksel bilgiler (AI için zorunlu)
        [Required]
        [Range(50, 250, ErrorMessage = "Boy 50 ile 250 cm arasında olmalıdır.")]
        public int Boy { get; set; }

        [Required]
        [Range(20, 300, ErrorMessage = "Kilo 20 ile 300 kg arasında olmalıdır.")]
        public int Kilo { get; set; }

        [Display(Name = "Hedefiniz")]
        public string Hedef { get; set; } // Kilo alma, verme, formda kalma

        [Display(Name = "Aktivite Seviyesi")]
        public string AktiviteSeviyesi { get; set; } // Sedanter, Hafif Aktif, Aktif, Çok Aktif

        [Display(Name = "Vücut Yağ Oranı (%)")]
        public float? YagOrani { get; set; }

        // Opsiyonel — Randevu planlaması için
        [Display(Name = "Tercih Edilen Antrenman Zamanı")]
        public string TercihEdilenZaman { get; set; }
    }
}
