using MangaReader.Dto;
using MangaReader.Dto.Manga;
using MangaReader.Models;

namespace MangaReader.Repositories
{
    public interface IMangaRepository
    {
        Task<PageResponse<IEnumerable<Manga>>> GetAllAsync(FilterMangaDto filterMangaDto);
        Task<PageResponse<IEnumerable<Manga>>> GetMostViewdAsync(PageableDto pageableDto);
        Task<PageResponse<IEnumerable<Manga>>> GetMostFollowedAsync(PageableDto pageableDto);
        Task<PageResponse<IEnumerable<Manga>>> GetNewestChapterAsync(PageableDto pageableDto);
        Task<Manga?> GetByIdAsync(int? mangaId, int chapterLimit);
        Task<Manga?> GetByIdAsync(int? mangaId);
        Task<Manga?> GetByIdWithGenresAsync(int? mangaId);
        Task<Manga?> GetByIdWithChaptersAsync(int? mangaId);
        Task<Manga?> GetBySlugAsync(string? mangaSlug);
        Task AddAsync(Manga manga);
        Task UpdateAsync(Manga manga);
        Task DeleteAsync(int mangaId);
        Task<bool> ExistsByIdAsync(int? mangaId);
        Task<bool> ExistsByTitleAsync(string? mangaTitle);
        Task<int> CountAllMangasAsync();
    }
}
