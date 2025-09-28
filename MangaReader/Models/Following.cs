using System.ComponentModel;

namespace MangaReader.Models
{
    public class Following
    {
        public required string UserId { get; set; }

        public required int MangaId { get; set; }

        public User User { get; set; } = default!;

        public Manga Manga { get; set; } = default!;

        [DisplayName("Theo dõi lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
