using MangaReader.Models;
using MangaReader.Services;
using MangaReader.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly IChapterService _chapterService;
        private readonly UserManager<User> _userManager;

        public ChaptersController(IChapterService chapterService, UserManager<User> userManager)
        {
            _chapterService = chapterService;
            _userManager = userManager;
        }


        // GET: Chapters/chapter-01-01
        [HttpGet("Chapters/{chapterSlug}-{chapterId}")]
        public async Task<IActionResult> Details(string? chapterSlug, int? chapterId)
        {
            if(chapterId == null || chapterSlug == null)
            {
                return NotFound();
            }

            string? userId = _userManager.GetUserId(User);
            ChapterDetailsViewModel viewModel = await _chapterService.GetChapterDetailsByIdAsync(chapterId, userId);
            if (!string.Equals(chapterSlug, viewModel.Chapter.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction(nameof(Details), new { chapterSlug = viewModel.Chapter.Slug, chapterId = viewModel.Chapter.Id });
            }

            return View(viewModel);
        }
    }
}
