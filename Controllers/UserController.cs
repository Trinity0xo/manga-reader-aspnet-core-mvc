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
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var users = new List<(User User, string? Role)>();

            if (loggedInUser != null)
            {
                var usersWithOutLoggedInUser = await _userManager.Users.Where(x => x.Id != loggedInUser.Id).ToListAsync();
                foreach (var user in usersWithOutLoggedInUser)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    string? firstRole = roles.FirstOrDefault();
                    users.Add((user, firstRole));
                }
            }
            return View(users);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.AllRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            string? firstRole = userRoles.FirstOrDefault();
            ViewBag.UserRole = firstRole;

            return View(user);
        }

        [HttpPost("edit"), ActionName("ConfirmEdit")]
        public async Task<IActionResult> Edit(User user, string SelectedRole)
        {
            var userDb = await _userManager.FindByIdAsync(user.Id);
            if(userDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _userManager.UpdateAsync(userDb);

                var userDbRoles = await _userManager.GetRolesAsync(userDb);

                if (userDbRoles.Any()) {
                    await _userManager.RemoveFromRolesAsync(userDb, userDbRoles);
                }


                if (!string.IsNullOrEmpty(SelectedRole)) {
                    await _userManager.AddToRoleAsync(userDb, SelectedRole);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.AllRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.UserRole = SelectedRole;

            return View(user);
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
