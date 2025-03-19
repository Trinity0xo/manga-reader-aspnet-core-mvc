using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public interface IComicRepository
    {
        Task<IEnumerable<Comic>> GetAllAsync(int pageSize = 15, int pageNumber = 0, string? searchValue = null, int genreId = 0);
        Task<Comic> GetByIdAsync(int id);
        Task AddAsync(Comic comic);
        Task UpdateAsync(Comic comic);
        Task DeleteAsync(int id);
        Task<int> CountFollowersAsync(int id);
        Task<IEnumerable<Comic>> GetFollowingAsync(string userId);
        Task FollowComicAsync(User userWithComics, Comic comic);
        Task RemoveFromFollowing(User userWithComics, Comic comic);
        Task<bool> IsFollowingAsync(User userWithComics, Comic comic);
        Task<int> GetTotalCountAsync(string? searchValue = null, int genreId = 0);
    }
}
