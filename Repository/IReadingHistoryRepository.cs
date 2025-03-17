using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public interface IReadingHistoryRepository
    {
        Task<IEnumerable<ReadingHistory>> GetReadingHistoryAsync(string id);
        Task RemoveFromHistoryAsync(string userId, Chapter chapter);

        Task AddToHistoryAsync(string userId, Chapter chapter);
    }
}
