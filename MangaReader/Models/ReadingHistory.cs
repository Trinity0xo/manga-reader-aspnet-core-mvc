using System.ComponentModel;

namespace MangaReader.Models
{
    public class ReadingHistory
    {
        public required string UserId { get; set; }

        public required int MangaId { get; set; }
        
        public required int ChapterId { get; set; }
        
        public User User { get; set; } = default!;

        public Manga Manga { get; set; } = default!;
        
        public Chapter Chapter { get; set; } = default!;

        [DisplayName("Đọc lúc")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
    