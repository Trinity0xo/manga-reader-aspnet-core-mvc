using MangaReader.Models;

namespace MangaReader.ViewModels
{
    public class MangaDetailsViewModel
    {
        public required Manga Manga { get; set; }
        public bool IsFollowed { get; set; } = false;
        public int TotalFollowed { get; set; } = 0;
        public int TotalViewed { get; set; } = 0;
    }
}
