using MangaReader.Dto;
using MangaReader.Dto.User;
using MangaReader.Models;

namespace MangaReader.Services
{
    public interface IUserService
    {
        Task<PageResponse<IEnumerable<User>>> GetAllUsersAsync(string currentUserId, PageableDto pageableDto);
        Task<User> GetUserDetailsByIdAsync(string? userId);
        Task<bool> ExistsByEmailAsync(string? email);
        Task CreateNewUserAsync(CreateUserDto createUserDto);
        Task<UpdateUserRoleDto> GetInfoForUpdateUserRoleAsync(string? currentUserId, string? userId);
        Task UpdateUserRoleAsync(string currentUserId, string userId, UpdateUserRoleDto updateUserRoleDto);
        Task<int> CountAllUsersAsync();
    }
}
