using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporMerkeziIsletmesi.Models
{
    public class Uye
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [Display(Name = "Identity Kullanıcı ID")]
        public string IdentityUserId { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; } = null!;

        [Display(Name = "Boy")]
        public int? Boy { get; set; }

        [Display(Name = "Kilo")]
        public int? Kilo { get; set; }

        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
