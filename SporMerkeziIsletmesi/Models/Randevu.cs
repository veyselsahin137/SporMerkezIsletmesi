using SporMerkeziIsletmesi.Models;
public class Randevu
{
    public int Id { get; set; }

    // Üye bilgisi
    public int UyeId { get; set; }
    public Uye Uye { get; set; }

    // Antrenör bilgisi
    public int AntrenorId { get; set; }
    public Antrenor Antrenor { get; set; }

    // Hizmet bilgisi (örn: Fitness, Pilates, Yoga...)
    public int HizmetId { get; set; }
    public Hizmet Hizmet { get; set; }

    // Randevu zamanı
    public DateTime Tarih { get; set; }

    // Durum: Beklemede / Onaylandı / İptal
    public string Durum { get; set; } = "Beklemede";
}
