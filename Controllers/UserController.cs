using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Controllers
{
    [Route("/admin/user")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? searchValue, int pageNumber = 0)
        {
            int pageSize = 15;

            var loggedInUser = await _userManager.GetUserAsync(User);

            var query = _userManager.Users
                .Where(u => u.Id != loggedInUser.Id).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(u => u.Email.Contains(searchValue));
            }

            var totalUsers = await query.CountAsync();

            query = query
                .OrderBy(u => u.UserName)
                .Skip(pageSize * pageNumber)
                .Take(pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
            ViewBag.searchValue = searchValue;

            var users = await query.ToListAsync();

            return View(users);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            string? userRole = userRoles.FirstOrDefault();
            ViewBag.userRole = userRole;

            return View(user);
        }

        [HttpPost("delete"), ActionName("ConfirmDelete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

    }
}
