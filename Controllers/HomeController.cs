using System.Diagnostics;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IComicRepository _comicRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly UserManager<User> _userManager;
        private readonly IChapterRepository _chapterRepository;
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        private readonly IReadingHistoryRepository _readingHistoryReposiory;

        public HomeController(ILogger<HomeController> logger, IComicRepository comicRepository, UserManager<User> userManager, IGenreRepository genreRepository, IChapterRepository chapterRepository,
        ApplicationDbContext context, IReadingHistoryRepository readingHistoryReposiory)
        {
            _logger = logger;
            _comicRepository = comicRepository;
            _userManager = userManager;
            _genreRepository = genreRepository;
            _chapterRepository = chapterRepository;
            _context = context;
            _readingHistoryReposiory = readingHistoryReposiory;

        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetAllAsync();
            ViewBag.Genres = genres;

            var comics = await _comicRepository.GetAllAsync();

            return View(comics);
        }

        [HttpGet("/comic/{id}"), ActionName("ComicDetails")]
        public async Task<IActionResult> Details(int id)
        {
            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            ViewBag.FollowerCount = await _comicRepository.CountFollowersAsync(comic.Id);
            ViewBag.IsFollowing = false;

            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser != null)
            {
                var userWithComics = await _context.Users
                    .Include(u => u.Comics)
                    .FirstOrDefaultAsync(u => u.Id == loggedInUser.Id);
                ViewBag.IsFollowing = await _comicRepository.IsFollowingAsync(userWithComics, comic);
            }

            return View(comic);
        }

        [HttpGet("/chapter/{id}"), ActionName("ChapterDetails")]
        public async Task<IActionResult> ChapterDetails(int id)
        {
            var selectedChapter = await _chapterRepository.GetByIdAsync(id);
            if (selectedChapter == null)
            {
                return NotFound();
            }

            var comicDetails = await _comicRepository.GetByIdAsync(selectedChapter.ComicId);
            ViewBag.ComicDetails = comicDetails;
            ViewBag.NextChapter = comicDetails.Chapters.Where(c => c.Id > selectedChapter.Id).OrderBy(c => c.Id).FirstOrDefault();
            ViewBag.PreviousChapter = comicDetails.Chapters.Where(c => c.Id < selectedChapter.Id).OrderByDescending(c => c.Id).FirstOrDefault();

            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser != null)
            {
                await _readingHistoryReposiory.AddToHistoryAsync(loggedInUser.Id, selectedChapter);

            }
            return View(selectedChapter);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
