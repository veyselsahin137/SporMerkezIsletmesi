using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporMerkeziIsletmesi.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UyeId { get; set; }

        [ForeignKey(nameof(UyeId))]
        public Uye Uye { get; set; } = null!;
    }
}
