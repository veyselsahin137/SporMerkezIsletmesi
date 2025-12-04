using System.ComponentModel.DataAnnotations;

namespace SporMerkeziIsletmesi.Models
{
    public class AntrenorHizmet
    {
        public int Id { get; set; }   // Kolaylık olsun diye tekil PK

        [Display(Name = "Antrenör")]
        public int AntrenorID { get; set; }

        [Display(Name = "Hizmet")]
        public int HizmetID { get; set; }

        // Navigation propertiler
        public Antrenor? Antrenor { get; set; }
        public Hizmet? Hizmet { get; set; }
    }
}
