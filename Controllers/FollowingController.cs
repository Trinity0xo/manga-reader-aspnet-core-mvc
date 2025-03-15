using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    [Authorize]
    [Route("/following")]
    public class FollowingController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IComicRepository _comicRepository;

        public FollowingController(UserManager<User> userManager, ApplicationDbContext context, IComicRepository comicRepository)
        {
            _userManager = userManager;
            _context = context;
            _comicRepository = comicRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var followingComics = await _comicRepository.GetFollowingAsync(loggedInUser.Id);

            return View(followingComics);
        }

        [HttpPost("add/{id}")]
        public async Task<IActionResult> Add(int id)
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null) { 
                return NotFound();
            }

            var userWithComics = await _context.Users
                .Include(u => u.Comics)
                .FirstOrDefaultAsync(u => u.Id == loggedInUser.Id);

            await _comicRepository.RemoveFromFollowing(userWithComics, comic);

            return RedirectToAction(nameof(Index));
        } 
    }
}
