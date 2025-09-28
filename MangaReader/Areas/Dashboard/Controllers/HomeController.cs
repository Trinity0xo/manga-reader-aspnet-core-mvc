using MangaReader.Services;
using MangaReader.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class HomeController : Controller
    {
        private readonly IHomeContentService _homeContentService;

        public HomeController(IHomeContentService homeContentService)
        {
            _homeContentService = homeContentService;
        }
        public async Task<IActionResult> Index()
        {
           DashboardContentViewModel dashboardContentDto = await _homeContentService.GetDashboardContentAsync();
            return View(dashboardContentDto);
        }
    }
}
