using MangaReader.Dto;
using MangaReader.Dto.Chapter;
using MangaReader.Models;
using MangaReader.ViewModels;

namespace MangaReader.Services
{
    public interface IChapterService
    {
        Task <bool> ExistsBySlugAsync(string? chapterSlug);
        Task<bool> ExistsByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug);
        Task<Chapter?> GetChapterBySlugAsync(string? chapterSlug);
        Task<Chapter> GetChapterDetailsByIdAsync(int? chapterId);
        Task<ChapterDetailsViewModel> GetChapterDetailsByIdAsync(int? chapterId, string? userId);
        Task<CreateChapterDto> GetInfoForCreateChapterAsync(int? mangaId);
        Task CreateNewChapterAsync(int mangaId, CreateChapterDto createChapterDto);
        Task<UpdateChapterDto> GetInfoForUpdateChapterAsync(int? chapterId);
        Task UpdateChapterAsync(int chapterId, UpdateChapterDto updateChapterDto);
        Task<Chapter> GetInfoForDeleteChapterAsync(int? chapterId);
        Task DeleteChapterAsync(int chapterId);
        Task<MangaChaptersViewModel> GetMangaChaptersAsync(int? mangaId, PageableDto pageableDto);
        Task<Chapter?> GetChapterByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug);
        Task<PageResponse<IEnumerable<Chapter>>> GetNewestChaptersAsync(PageableDto pageableDto);
        Task<int> CountAllChaptersAsync();
    }
}
