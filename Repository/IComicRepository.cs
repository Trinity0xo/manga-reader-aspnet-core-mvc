using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public interface IComicRepository
    {
        Task<IEnumerable<Comic>> GetAllAsync();
        Task<Comic> GetByIdAsync(int id);
        Task AddAsync(Comic comic);
        Task UpdateAsync(Comic comic);
        Task DeleteAsync(int id);
    }
}
