namespace MangaReader.Utils
{
    public class DateTimeUtils
    {
        public static string TimeAgo(DateTime dateTime)
        {
            var ts = DateTime.Now - dateTime;
            if (ts.TotalSeconds < 60) return $"{ts.Seconds} giây trước";
            if (ts.TotalMinutes < 60) return $"{ts.Minutes} phút trước";
            if (ts.TotalHours < 24) return $"{ts.Hours} tiếng trước";
            if (ts.TotalDays < 30) return $"{ts.Days} ngày trước";
            if (ts.TotalDays < 365) return $"{(int)(ts.TotalDays / 30)} thấng trước";
            return $"{(int)(ts.TotalDays / 365)} năm trước";
        }
    }
}
