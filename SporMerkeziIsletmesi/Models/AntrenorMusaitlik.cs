using System;
using System.ComponentModel.DataAnnotations;

namespace SporMerkeziIsletmesi.Models
{
    public class AntrenorMusaitlik
    {
        public int Id { get; set; }

        [Display(Name = "Antrenör")]
        public int AntrenorID { get; set; }
        public Antrenor? Antrenor { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Gün")]
        public string Gun { get; set; }  // Örn: "Pazartesi", "Salı" vs.

        [Required]
        [Display(Name = "Başlangıç Saati")]
        public TimeSpan BaslangicSaati { get; set; }

        [Required]
        [Display(Name = "Bitiş Saati")]
        public TimeSpan BitisSaati { get; set; }
    }
}
