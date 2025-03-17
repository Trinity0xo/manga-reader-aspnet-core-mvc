using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    [Authorize]
    [Route("/history")]
    public class HistoryController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IComicRepository _comicRepository;
        private readonly IChapterRepository _chapterRepository;

        private readonly IReadingHistoryRepository _readingHistoryReposiory;

        public HistoryController(UserManager<User> userManager, ApplicationDbContext context, IComicRepository comicRepository, IChapterRepository chapterRepository, IReadingHistoryRepository readingHistoryReposiory)
        {
            _userManager = userManager;
            _chapterRepository = chapterRepository;
            _context = context;
            _comicRepository = comicRepository;
            _readingHistoryReposiory = readingHistoryReposiory;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var readingHistory = await _readingHistoryReposiory.GetReadingHistoryAsync(loggedInUser.Id);

            return View(readingHistory);
        }

        [HttpPost("/remove/{id}"), ActionName("ConfirmRemove")]
        public async Task<IActionResult> Remove(int id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var selectedChapter = await _chapterRepository.GetByIdAsync(id);
            if (selectedChapter == null)
            {
                return NotFound();
            }


            await _readingHistoryReposiory.RemoveFromHistoryAsync(loggedInUser.Id, selectedChapter);

            var referer = Request.Headers["Referer"].ToString();

            return Redirect(referer);
        }

    }
}