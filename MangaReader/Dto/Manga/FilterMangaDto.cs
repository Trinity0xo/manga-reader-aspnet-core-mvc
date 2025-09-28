using MangaReader.Utils.Constants;

namespace MangaReader.Dto.Manga
{
    public class FilterMangaDto : PageableDto
    {
        public string? AuthorName { get; set; }

        public List<string>? Genres { get; set; } 

        public List<MangaStatusEnum>? Status { get; set; }

        public int ActiveFilterCount
        {
            get
            {
                int count = 0;

                if (Genres != null && Genres.Count != 0)
                    count += Genres.Count;

                if (Status != null && Status.Count != 0)
                    count += Status.Count;

                return count;
            }
        }
    }
}
