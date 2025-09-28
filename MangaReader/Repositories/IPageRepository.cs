using MangaReader.Models;

namespace MangaReader.Repositories
{
    public interface IPageRepository
    {
        Task<IEnumerable<Page>> GetAllAsync(int chapterId);
        Task<Page?> GetByIdAsync(int PageId);
        Task AddAsync(Page page);
        Task UpdateAsync(Page page);
        Task DeleteAsync(int pageId);
        Task DeleteAllAsync(List<Page> pages);
        Task AddBulkAsync(List<Page> pages);
    }
}
