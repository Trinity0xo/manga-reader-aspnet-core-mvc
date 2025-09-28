using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.Repositories
{
    public interface IChapterRepository
    {
        Task<PageResponse<IEnumerable<Chapter>>> GetAllAsync(PageableDto pageableDto);
        Task<PageResponse<IEnumerable<Chapter>>> GetAllAsync(int? mantaId, PageableDto pageableDto);
        Task<IEnumerable<Chapter>> GetAllAsync(int? mangaId);
        Task<Chapter?> GetByIdAsync(int? chapterId);
        Task<Chapter?> GetByMangaIdAndChapterId(int? mangaId, int? chapterId);
        Task<Chapter?> GetBySlugAsync(string? chapterSlug);
        Task<Chapter?> GetByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug);
        Task UpdateAsync(Chapter chapter);
        Task DeleteAsync(int chapterId);
        Task<bool> ExistsById(int? chapterId);
        Task<bool> ExistsBySlugAsync(string? chapterSlug);
        Task<bool> ExistsByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug);
        Task<int> CountAllChaptersAsync();
        Task<Chapter?> GetPreviousChapterByMangaIdAndChapterIdAsync(int? mangaId, int? ChapterId);
        Task<Chapter?> GetNextChapterByMangaIdAndChapterIdAsync(int? mangaId, int? ChapterId);
    }
}
