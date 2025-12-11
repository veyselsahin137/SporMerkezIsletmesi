using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporMerkeziIsletmesi.Models
{
    [Table("Uyeler")]
    public class Uye
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // -------------------------
        // Temel Bilgiler
        // -------------------------

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        // Identity User bağlantısı
        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string KullaniciId { get; set; }

        // -------------------------
        // Salon Bilgisi
        // -------------------------

        [Required]
        public int SalonId { get; set; }

        [ForeignKey("SalonId")]
        public Salon Salon { get; set; }

        // -------------------------
        // Kişisel Bilgiler
        // -------------------------

        [Required]
        [Range(1, 120)]
        [Display(Name = "Yaş")]
        public int Yas { get; set; }

        [Phone]
        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "Telefon Numarası")]
        public string Telefon { get; set; }

        // -------------------------
        // Fiziksel Bilgiler
        // -------------------------

        [Required]
        [Range(50, 250)]
        [Display(Name = "Boy (cm)")]
        public int Boy { get; set; }

        [Required]
        [Range(20, 300)]
        [Display(Name = "Kilo (kg)")]
        public int Kilo { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Hedefiniz")]
        public string Hedef { get; set; } // Kilo alma, verme, formda kalma

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Aktivite Seviyesi")]
        public string AktiviteSeviyesi { get; set; }

        [Display(Name = "Vücut Yağ Oranı (%)")]
        public float? YagOrani { get; set; }

        // -------------------------
        // Randevu Planlama
        // -------------------------

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Tercih Edilen Antrenman Zamanı")]
        public string TercihEdilenZaman { get; set; }
    }
}
