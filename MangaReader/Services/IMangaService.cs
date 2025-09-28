using MangaReader.Dto;
using MangaReader.Dto.Manga;
using MangaReader.Models;
using MangaReader.ViewModels;

namespace MangaReader.Services
{
    public interface IMangaService
    {
        Task<PageResponse<IEnumerable<Manga>>> GetMostFollowedMangasAsync(PageableDto pageableDto);
        Task<PageResponse<IEnumerable<Manga>>> GetMostViewedMangasAsync(PageableDto pageableDto);
        Task<PageResponse<IEnumerable<Manga>>> GetNewestChapterMangasAsync(PageableDto pageableDto);
        Task<PageResponse<IEnumerable<Manga>>> GetAllMangasAsync(FilterMangaDto filterMangaDto);
        Task<Manga> GetMangaDetailsByIdAsync(int? mangaId);
        Task<MangaDetailsViewModel> GetMangaDetailsByIdAsync(int? mangaId, string? userId);
        Task CreateNewMangaAsync(CreateMangaDto createMangaDto);
        Task<UpdateMangaDto> GetMangaInfoForUpdateAsync(int? mangaId);
        Task UpdateMangaAsync(int mangaId, UpdateMangaDto updateMangaDto);
        Task<Manga> GetMangaInfoForDeleteAsync(int? mangaId);
        Task DeleteMangaAsync(int mangaId);
        Task<bool> ExistsByTitleAsync(string? title);
        Task<Manga?> GetMangaBySlugAsync(string? slug);
        Task<int> CountAllMangasAsync();
    }
}
