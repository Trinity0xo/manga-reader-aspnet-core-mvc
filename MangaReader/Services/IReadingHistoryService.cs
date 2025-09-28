using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.Services
{
    public interface IReadingHistoryService
    {
        Task<PageResponse<IEnumerable<ReadingHistory>>> GetUserReadingHistoriesAsync(string? userId, PageableDto pageableDto);
    }
}
