using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IComicRepository _comicRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly UserManager<User> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IComicRepository comicRepository, UserManager<User> userManager, IGenreRepository genreRepository)
        {
            _logger = logger;
            _comicRepository = comicRepository;
            _userManager = userManager;
            _genreRepository = genreRepository;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetAllAsync();
            ViewBag.Genres = genres;

            var comics = await _comicRepository.GetAllAsync();

            return View(comics);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null) { 
                return NotFound();
            }

            ViewBag.FollowerCount = await _comicRepository.CountFollowersAsync(comic.Id);

            return View(comic);
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
