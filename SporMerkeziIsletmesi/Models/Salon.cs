using System;
using System.ComponentModel.DataAnnotations;

namespace SporMerkeziIsletmesi.Models
{
    public class Salon
    {
        public int SalonID { get; set; }   // Primary Key

        [Required]
        [MaxLength(100)]
        [Display(Name = "Salon Adı")]
        public string SalonAdi { get; set; }

        [MaxLength(200)]
        [Display(Name = "Adres")]
        public string? Adres { get; set; }

        [Required]
        [Display(Name = "Açılış Saati")]
        public TimeSpan AcilisSaati { get; set; }

        [Required]
        [Display(Name = "Kapanış Saati")]
        public TimeSpan KapanisSaati { get; set; }

        public ICollection<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
    }
}

