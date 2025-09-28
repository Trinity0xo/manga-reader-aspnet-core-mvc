using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.ViewModels
{
    public class MangaChaptersViewModel
    {
        public required int MangaId { get; set; }
        public required string MangaTitle { get; set; }
        public required string MangaSlug { get; set; }
        public required PageResponse<IEnumerable<Chapter>> Chapers { get; set; }
    }
}
