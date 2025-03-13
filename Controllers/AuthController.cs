using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace WEBTRUYEN.Controllers
{
    public class AuthController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
