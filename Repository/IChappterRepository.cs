using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public interface IChapterRepository
    {
        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<Chapter> GetByIdAsync(int id);
        Task AddAsync(Chapter chapter);
        Task UpdateAsync(Chapter chapter);
        Task DeleteAsync(int id);
    }
}
