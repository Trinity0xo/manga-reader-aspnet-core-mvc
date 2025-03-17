using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTRUYEN.Models
{
    public class ReadingHistory
    {
        [ForeignKey("User")]
        public string UsersId { get; set; }
        [ForeignKey("Chapter")]
        public int ChaptersId { get; set; }
        public User User { get; set; }
        public Chapter Chapter { get; set; }
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    }
}