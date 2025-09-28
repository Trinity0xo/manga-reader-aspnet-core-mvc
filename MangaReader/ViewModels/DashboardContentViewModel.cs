using MangaReader.Models;


namespace MangaReader.ViewModels
{
    public class DashboardContentViewModel
    {
        public IEnumerable<Chapter> NewestChapters { get; set; } = [];
        public int ChapterCount { get; set; } = 0;
        public int MangaCount { get; set; } = 0;
        public int GenreCount { get; set; } = 0;
        public int UserCount { get; set; } = 0;
    }
}
