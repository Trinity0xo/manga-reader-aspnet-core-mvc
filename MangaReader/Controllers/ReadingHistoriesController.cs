using MangaReader.Dto;
using MangaReader.Models;
using MangaReader.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Controllers
{
    public class ReadingHistoriesController : Controller
    {
        private readonly IReadingHistoryService _readingHistoryService;
        private readonly UserManager<User> _userManager;

        public ReadingHistoriesController(IReadingHistoryService readingHistoryService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _readingHistoryService = readingHistoryService;
        }

        // GET: ReadingHistories
        public async Task<IActionResult> Index(PageableDto pageableDto)
        {
            string? userId = _userManager.GetUserId(User)!;

            PageResponse<IEnumerable<ReadingHistory>> readingHistories = await _readingHistoryService.GetUserReadingHistoriesAsync(userId, pageableDto);
            ViewBag.PageableDto = pageableDto;

            return View(readingHistories);
        }
    }
}
