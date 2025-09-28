using MangaReader.Models;

namespace MangaReader.ViewModels
{
    public class ChapterDetailsViewModel
    {
        public Chapter? NextChapter { get; set; }
        public Chapter? PreviousChapter { get; set; }
        public required Chapter Chapter { get; set; }
        public IEnumerable<Chapter> Chapters { get; set; } = [];
    }
}
