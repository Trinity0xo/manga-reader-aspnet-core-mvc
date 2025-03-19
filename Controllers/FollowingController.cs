using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    [Authorize]
    [Route("/followings")]
    public class FollowingController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IComicRepository _comicRepository;
        private readonly IGenreRepository _genreRepository;

        public FollowingController(UserManager<User> userManager, ApplicationDbContext context, IComicRepository comicRepository, IGenreRepository genreRepository)
        {
            _userManager = userManager;
            _context = context;
            _comicRepository = comicRepository;
            _genreRepository = genreRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // var genres = await _genreRepository.GetAllNoPaginateAsync();
            // ViewBag.Genres = genres;

            var loggedInUser = await _userManager.GetUserAsync(User);

            var followingComics = await _comicRepository.GetFollowingAsync(loggedInUser.Id);

            return View(followingComics);
        }

        [HttpPost("following/{id}"), ActionName("ConfirmFollowing")]
        public async Task<IActionResult> Following(int id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            var userWithComics = await _context.Users
                .Include(u => u.Comics)
                .FirstOrDefaultAsync(u => u.Id == loggedInUser.Id);

            await _comicRepository.FollowComicAsync(userWithComics, comic);

            var referer = Request.Headers["Referer"].ToString();

            return Redirect(referer);

            // return RedirectToAction("ComicDetails", "Home", new { id = comic.Id });
        }

        [HttpPost("unfollowing/{id}"), ActionName("ConfirmUnfollowing")]
        public async Task<IActionResult> UnFollowing(int id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            var userWithComics = await _context.Users
                .Include(u => u.Comics)
                .FirstOrDefaultAsync(u => u.Id == loggedInUser.Id);

            await _comicRepository.RemoveFromFollowing(userWithComics, comic);

            var referer = Request.Headers["Referer"].ToString();

            return Redirect(referer);

            // return RedirectToAction("ComicDetails", "Home", new { id = comic.Id });
        }
    }
}
