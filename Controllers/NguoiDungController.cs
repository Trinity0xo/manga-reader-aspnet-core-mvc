using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Controllers
{
    [Route("/admin/nguoidung")]
    [Authorize(Roles = "admin")]
    public class NguoiDungController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;

        public NguoiDungController(UserManager<NguoiDung> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var nguoiDung = await _userManager.Users.ToListAsync();
            return View(nguoiDung);
        }

    }
}
