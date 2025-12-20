namespace SporMerkeziIsletmesi.Helpers
{
    public static class DateHelper
    {
        public static string GunCevir(DateTime tarih)
        {
            return tarih.DayOfWeek switch
            {
                DayOfWeek.Monday => "Pazartesi",
                DayOfWeek.Tuesday => "Salı",
                DayOfWeek.Wednesday => "Çarşamba",
                DayOfWeek.Thursday => "Perşembe",
                DayOfWeek.Friday => "Cuma",
                DayOfWeek.Saturday => "Cumartesi",
                DayOfWeek.Sunday => "Pazar",
                _ => ""
            };
        }

        public static DateTime GunToDate(string gun)
        {
            var today = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                var d = today.AddDays(i);
                if (GunCevir(d) == gun)
                    return d;
            }

            return today;
        }
    }
}
