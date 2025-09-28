using MangaReader.Models;

namespace MangaReader.ViewModels
{
    public class HomeContentViewModel
    {
        public IEnumerable<Manga> NewestChapterMangas { get; set; } = [];
        public IEnumerable<Manga> MostViewdMangas { get; set; } = [];
        public IEnumerable<Manga> NewestMangas { get; set; } = [];
        public IEnumerable<Manga> MostFollowedManga { get; set; } = [];
    }
}
