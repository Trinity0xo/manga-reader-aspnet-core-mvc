using WEBTRUYEN.Models;
namespace WEBTRUYEN.Repository
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllAsync(int pageSize = 0, int pageNumber = 0, string? searchValue = null);
        Task<IEnumerable<Genre>> GetAllNoPaginateAsync();
        Task<Genre> GetByIdAsync(int id);
        Task AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(int id);
        Task<List<Genre>?> GetByIdAsync(List<int> genreIds);
        Task<int> GetTotalCountAsync(string? searchValue = null);
    }
}