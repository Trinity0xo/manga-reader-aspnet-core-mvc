using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.Services
{
    public interface IRoleService
    {
        Task<PageResponse<IEnumerable<Role>>> GetAllRolesAsync(PageableDto pageableDto);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleDetailsByIdAsync(string? roleId);
    }
}
