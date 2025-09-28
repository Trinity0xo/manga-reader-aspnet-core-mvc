using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.Services
{
    public interface IFollowingService
    {
        Task<string> FollowingAMangaAsync(int mangaId, string userId);
        Task<string> UnfollowingAMangaAsync(int mangaId, string userId);
        Task <PageResponse<IEnumerable<Following>>> GetUserFollowingsAsync(string? userId, PageableDto pageableDto);
    }
}
