using MangaReader.Dto;
using MangaReader.Dto.Genre;
using MangaReader.Dto.User;
using MangaReader.Models;
using MangaReader.Services.Implementations;
using MangaReader.Services;
using MangaReader.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(UserManager<User> userManager, IUserService userService, IRoleService roleService)
        {
            _userManager = userManager;
            _userService = userService;
            _roleService = roleService;
        }

        // GET: Users
        public async Task<IActionResult> Index(PageableDto pageableDto)
        {
            string userId = _userManager.GetUserId(User)!;

            PageResponse<IEnumerable<User>> users = await _userService.GetAllUsersAsync(userId, pageableDto);
            ViewBag.PageableDto = pageableDto;
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _userService.GetUserDetailsByIdAsync(id);

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleService.GetAllRolesAsync();
                return View(createUserDto);
            }

            bool existEmail = await _userService.ExistsByEmailAsync(createUserDto.Email);
            if (existEmail)
            {
                ViewBag.Roles = await _roleService.GetAllRolesAsync();
                ModelState.AddModelError(nameof(createUserDto.Email), "Email đã tồn tại");
                return View(createUserDto);
            }

            await _userService.CreateNewUserAsync(createUserDto);
            TempData["SuccessMessage"] = "Tạo người dùng mới thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            string currentUserId = _userManager.GetUserId(User)!;

            UpdateUserRoleDto updateUserRoleDto = await _userService.GetInfoForUpdateUserRoleAsync(currentUserId, id);
            ViewBag.Roles = await _roleService.GetAllRolesAsync();

            return View(updateUserRoleDto);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateUserRoleDto updateUserRoleDto)
        {
            if (id != updateUserRoleDto.Id)
            {
                return NotFound();
            }

            string currentUserId = _userManager.GetUserId(User)!;

            if (!ModelState.IsValid)
            {
                UpdateUserRoleDto newUpdateUserRoleDto = await _userService.GetInfoForUpdateUserRoleAsync(currentUserId, updateUserRoleDto.Id);
                updateUserRoleDto.Email = newUpdateUserRoleDto.Email;
                ViewBag.Roles = await _roleService.GetAllRolesAsync();

                return View(updateUserRoleDto);
            }

            await _userService.UpdateUserRoleAsync(currentUserId, id, updateUserRoleDto);
            TempData["SuccessMessage"] = "Cập nhật người dùng thành công";
            return RedirectToAction(nameof(Details), new { id = updateUserRoleDto.Id });
        }
    }
}
