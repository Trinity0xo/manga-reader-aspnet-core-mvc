using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.Repositories
{
    public interface IFollowRepository
    {
        Task AddAsync(Following following);
        Task DeleteAsync(string userId, int mangaId);
        Task<bool> ExistsByUserIdAndMangaIdAsync(string userId, int mangaId);
        Task<PageResponse<IEnumerable<Following>>> GetAllAsync(string userId, PageableDto pageableDto);
        Task<int> CountByMangaId(int? mangaId);
    }
}
