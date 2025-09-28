using MangaReader.Services;
using MangaReader.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeContentService _homeContentService;

        public HomeController(IHomeContentService homeContentService)
        {
            _homeContentService = homeContentService;
        }

        public async Task<IActionResult> Index()
        {
            HomeContentViewModel homeContentDto = await _homeContentService.GetHomeContentAsync();
            return View(homeContentDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
