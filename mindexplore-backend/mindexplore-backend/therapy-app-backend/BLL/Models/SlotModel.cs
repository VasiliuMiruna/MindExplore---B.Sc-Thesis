using System.Globalization;

namespace therapy_app_backend.BLL.Models
{
    public class SlotModel
    {
        //private static readonly CultureInfo RomanianCulture = new CultureInfo("ro-RO");
        private static readonly TimeZoneInfo RomanianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest");
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Available { get; set; }
        /*     public string GetStartTimeString()
             {
                 return StartTime.ToString("dd.MM.yyyy HH:mm", RomanianCulture);
             }
             public string GetEndTimeString()
             {
                 return EndTime.ToString("dd.MM.yyyy HH:mm", RomanianCulture);
             }*/
        public string GetStartTimeString()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(StartTime, RomanianTimeZone).ToString("dd.MM.yyyy HH:mm zzz");
        }

        public string GetEndTimeString()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(EndTime, RomanianTimeZone).ToString("dd.MM.yyyy HH:mm zzz");
        }
    }
}
