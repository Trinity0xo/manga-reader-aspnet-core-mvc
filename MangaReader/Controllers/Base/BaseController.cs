using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Controllers.Base
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
