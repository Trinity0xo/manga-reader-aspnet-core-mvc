using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public interface IPageRepository
    {
        Task<IEnumerable<Page>> GetAllAsync();
        Task<Page> GetByIdAsync(int id);
        Task AddAsync(Page page);
        Task UpdateAsync(Page page);
        Task DeleteAsync(int id);
        Task DeleteAllAsync(List<Page> pages);

        Task AddBulkAsync(List<Page> pages);
    }
}
