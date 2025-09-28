using MangaReader.Dto;
using MangaReader.Models;
using MangaReader.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;
        
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: Roles
        public async Task<IActionResult> Index(PageableDto pageableDto)
        {
            PageResponse<IEnumerable<Role>> roles = await _roleService.GetAllRolesAsync(pageableDto);
            ViewBag.PageableDto = pageableDto;
            return View(roles);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Role role = await _roleService.GetRoleDetailsByIdAsync(id);
        
            return View(role);
        }
    }
}
