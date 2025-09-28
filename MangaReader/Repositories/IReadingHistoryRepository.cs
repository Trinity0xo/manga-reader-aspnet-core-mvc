using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.Repositories
{
    public interface IReadingHistoryRepository
    {
        Task AddAsync(ReadingHistory readingHistory);
        Task UpdateAsync(ReadingHistory readingHistory);
        Task DeleteAllByChapterIdAsync(int chapterId);
        Task<ReadingHistory?> GetByUserIdAndMangaIdAsync(int? mangaId, string? userId);
        Task<PageResponse<IEnumerable<ReadingHistory>>> GetAllAsync(string userId, PageableDto pageableDto);
        Task<int> CountByMangaId(int? mangaId);
    }
}
