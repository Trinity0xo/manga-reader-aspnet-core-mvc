using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IComicRepository _comicRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(IComicRepository comicRepository, IGenreRepository genreRepository, UserManager<User> userManager)
        {
            _comicRepository = comicRepository;
            _genreRepository = genreRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ComicCount = await _comicRepository.GetTotalCountAsync();
            ViewBag.GenreCount = await _genreRepository.GetTotalCountAsync();
            ViewBag.UserCount = (await _userManager.GetUsersInRoleAsync("user")).Count;

            return View();
        }
    }
}
