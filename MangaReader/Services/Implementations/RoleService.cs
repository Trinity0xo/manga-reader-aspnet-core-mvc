using MangaReader.Dto;
using MangaReader.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<MangaService> _logger;

        public RoleService(RoleManager<Role> roleManager, ILogger<MangaService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<PageResponse<IEnumerable<Role>>> GetAllRolesAsync(PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _roleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(pageableDto.Search))
            {
                string keyword = pageableDto.Search.ToLower();
                query = query.Where(u => u.DisplayName.ToLower().Contains(keyword));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PageResponse<IEnumerable<Role>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<Role> GetRoleDetailsByIdAsync(string? roleId)
        {
            Role? role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                _logger.LogError("Failed to fetch role details. Role not found with ID: {RoleId}", roleId);
                throw new KeyNotFoundException("Không tìm thấy vai trò");
            }
            
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
