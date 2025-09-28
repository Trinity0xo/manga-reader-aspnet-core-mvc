using MangaReader.Dto;
using MangaReader.Models;
using MangaReader.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Controllers
{
    [Authorize]
    public class FollowingsController : Controller
    {
        private readonly IFollowingService _followingService;
        private readonly UserManager<User> _userManager;

        public FollowingsController(IFollowingService followingService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _followingService = followingService;
        }


        // GET: Followings
        public async Task<IActionResult> Index(PageableDto pageableDto)
        {
            string userId = _userManager.GetUserId(User)!;

            PageResponse<IEnumerable<Following>> followings = await _followingService.GetUserFollowingsAsync(userId, pageableDto);
            ViewBag.PageableDto = pageableDto;

            return View(followings);
        }

        // POST: Followings/Follow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Follow(int mangaId)
        {
            string userId = _userManager.GetUserId(User)!;

            string mangaSlug = await _followingService.FollowingAMangaAsync(mangaId, userId);

            return RedirectToAction(
                "Details",         
                "Mangas",          
                new { mangaSlug, mangaId }
            );
        }

        // POST: Followings/Unfollow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unfollow(int mangaId)
        {
            string userId = _userManager.GetUserId(User)!;

            string mangaSlug = await _followingService.UnfollowingAMangaAsync(mangaId, userId);

            return RedirectToAction(
                "Details",          
                "Mangas",         
                new { mangaSlug, mangaId }
            );
        }
    }
}
