namespace SporMerkeziIsletmesi.Models.ViewModels
{
    public class RandevuSlotVM
    {
        public int AntrenorId { get; set; }
        public int HizmetId { get; set; }

        public DateTime Tarih { get; set; }

        public TimeSpan Baslangic { get; set; }
        public TimeSpan Bitis { get; set; }

        public string GunYazi { get; set; } = "";
        public string GunAdi { get; set; } = "";
        public string Etiket { get; set; } = "";
        // Örn: "30.12.2025 Salı 13:00 - 14:00"
    }
}
