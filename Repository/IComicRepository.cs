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
        Task<int> CountFollowersAsync(int id);
        Task<IEnumerable<Comic>> GetFollowingAsync(string userId);
        Task FollowComicAsync(User userWithComics, Comic comic);
        Task RemoveFromFollowing(User userWithComics, Comic comic);
    }
}
