
using MangaReader.Dto;
using MangaReader.Models;
using MangaReader.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MangaReader.Services.Implementations
{
    public class FollowingService : IFollowingService
    {
        private readonly IFollowRepository _followingRepository;
        private readonly IMangaRepository _mangaRepository;
        private readonly ILogger<FollowingService> _logger;
        private readonly UserManager<User> _userManager;
        
        public FollowingService(IMangaRepository mangaRepository, IFollowRepository followRepository, ILogger<FollowingService> logger, UserManager<User> userManager)
        {
            _followingRepository = followRepository;
            _mangaRepository = mangaRepository;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<string> FollowingAMangaAsync(int mangaId, string userId)
        {
            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if(manga == null)
            {
                _logger.LogError("Failed to follow manga. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                _logger.LogError("Failed to follow manga. User not found with ID: {UserId}", userId);
                throw new UnauthorizedAccessException();
            }

            Following following = new()
            {
                MangaId = mangaId,
                UserId = userId,
            };

            await _followingRepository.AddAsync(following);

            return manga.Slug;
        }

        public async Task<string> UnfollowingAMangaAsync(int mangaId, string userId)
        {
            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to unfollow manga. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Failed to unfollow manga. User not found with ID: {UserId}", userId);
                throw new UnauthorizedAccessException();
            }

            await _followingRepository.DeleteAsync(userId, mangaId);

            return manga.Slug;
        }

        public async Task<PageResponse<IEnumerable<Following>>> GetUserFollowingsAsync(string? userId, PageableDto pageableDto)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Failed to fetch followed manga. User not found with ID: {UserId}", userId);
                throw new UnauthorizedAccessException();
            }

            return await _followingRepository.GetAllAsync(userId, pageableDto);
        }
    }
}
