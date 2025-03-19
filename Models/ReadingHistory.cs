using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTRUYEN.Models
{
    public class ReadingHistory
    {
        public string UserId { get; set; }

        public int ChapterId { get; set; }

        public User User { get; set; }

        public Chapter Chapter { get; set; }

        [DisplayName("Đọc lúc")]
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    }
}