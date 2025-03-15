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

            var followingComics = await _context.Users
                    .Include(c => c.Comics
                        .OrderByDescending(c => c.Chapters.OrderByDescending(c => c.CreatedDate)
                            .Select(c => c.CreatedDate).FirstOrDefault()))
                    .ThenInclude(c => c.Chapters.OrderByDescending(ch => ch.CreatedDate))
                    .FirstOrDefaultAsync(u => u.Id == loggedInUser.Id);

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

            var followingComics = await _context.Users
                .Include(u => u.Comics)
                .FirstOrDefaultAsync(u => u.Id == loggedInUser.Id);

            if (followingComics != null)
            {
                followingComics.Comics.Add(comic);
                await _context.SaveChangesAsync();
            }

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

            var followingComics = await _context.Users
                .Include(u => u.Comics)
                .FirstOrDefaultAsync(u => u.Id == loggedInUser.Id);

            if (followingComics != null)
            {
                followingComics.Comics.Remove(comic);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        } 
    }
}
