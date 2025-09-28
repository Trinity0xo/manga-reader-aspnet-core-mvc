using MangaReader.Dto;
using MangaReader.Models;
using MangaReader.Repositories;
using Microsoft.AspNetCore.Identity;

namespace MangaReader.Services.Implementations
{
    public class ReadingHistoryService : IReadingHistoryService
    {
        private readonly IReadingHistoryRepository _readingHistoryRepository;
        private readonly ILogger<ReadingHistoryService> _logger;
        private readonly UserManager<User> _userManager;

        public ReadingHistoryService(UserManager<User> userManager, ILogger<ReadingHistoryService>logger, IReadingHistoryRepository readingHistoryRepository)
        {
            _readingHistoryRepository = readingHistoryRepository;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<PageResponse<IEnumerable<ReadingHistory>>> GetUserReadingHistoriesAsync(string? userId, PageableDto pageableDto)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Failed to fetch reading history. User not found with ID: {UserId}", userId);
                throw new UnauthorizedAccessException();
            }

            return await _readingHistoryRepository.GetAllAsync(userId, pageableDto);
        }
    }
}
