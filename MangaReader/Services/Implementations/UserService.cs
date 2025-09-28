using MangaReader.Dto;
using MangaReader.Dto.User;
using MangaReader.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IFileService _fileService;
        private readonly ILogger<MangaService> _logger;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IFileService fileService, ILogger<MangaService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<PageResponse<IEnumerable<User>>> GetAllUsersAsync(string currentUserId, PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(pageableDto.Search))
            {
                string keyword = pageableDto.Search.ToLower();
                query = query.Where(u => u.Email.ToLower().Contains(keyword));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                 .Where(u => u.Id != currentUserId)
                 .Skip((page - 1) * limit)
                 .Take(limit)
                 .ToListAsync();


            return new PageResponse<IEnumerable<User>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<User> GetUserDetailsByIdAsync(string? userId)
        {
            User? user = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                _logger.LogError("Failed to fetch user details. User not found with ID: {UserId}", userId);
                throw new KeyNotFoundException("Không tìm thấy người dùng");
            }

            return user;
        }

        public async Task<bool> ExistsByEmailAsync(string? email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task CreateNewUserAsync(CreateUserDto createUserDto)
        {
            User user = new()
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                UserName = createUserDto.Email
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded) {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create new user. Errors: {Errors}", errors);
                throw new InvalidOperationException("Không thể tạo người dùng");
            }

            Role? role = await _roleManager.FindByIdAsync(createUserDto.RoleId);
            if (role == null) {
                _logger.LogError("Failed to create new user. Role not found with ID: {RoleId}", createUserDto.RoleId);
                throw new KeyNotFoundException("Không tìm thấy vai trò");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!roleResult.Succeeded) {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create new user. Errors: {Errors}", errors);
                throw new InvalidOperationException("Không thể gán vai trò cho người dùng");
            }
        }

        public async Task<UpdateUserRoleDto> GetInfoForUpdateUserRoleAsync(string? currentUserId, string? userId)
        {
            if (currentUserId == userId)
            {
                _logger.LogError("Failed to fetch user info for update role. Self role update. CurrentUserId: {CurrentUserId}, TargetUserId: {TargetUserId}", currentUserId, userId);
                throw new InvalidOperationException("Không thể cập nhật vai trò");
            }

            User? user = await GetUserDetailsByIdAsync(userId);

            string? roleId = user.UserRoles?.FirstOrDefault()?.Role?.Id;

            UpdateUserRoleDto updateUserRoleDto = new()
            {
                Id = userId,
                Email = user.Email,
                RoleId = roleId
            };

            return updateUserRoleDto;
        }

        public async Task<User> UpdateUserRoleAsync(string currentUserId, string userId, UpdateUserRoleDto updateUserRoleDto)
        {
            if (currentUserId == userId)
            {
                _logger.LogError("Failed to update user role. Self role update. CurrentUserId: {CurrentUserId}, TargetUserId: {TargetUserId}", currentUserId, userId);
                throw new InvalidOperationException("Không thể cập nhật vai trò");
            }

            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Failed to update user role. User not found with ID: {UserId}", userId);
                throw new KeyNotFoundException("Không tìm thấy người dùng");
            }

            Role? role = await _roleManager.FindByIdAsync(updateUserRoleDto.RoleId);
            if (role == null)
            {
                _logger.LogError("Failed to update user role. Role not found with ID: {RoleId}", updateUserRoleDto.RoleId);
                throw new KeyNotFoundException("Không tìm thấy vai trò");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to update user role. Errors: {Errors}", errors);
                    throw new InvalidOperationException("Không thể cập nhật vai trò");
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update user role. Errors: {Errors}", errors);
                throw new InvalidOperationException("Không thể cập nhật vai trò");
            }

            return user;
        }

        public async Task<int> CountAllUsersAsync()
        {
            // todo: don't hard code
            return await _userManager.Users
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "User"))
                .CountAsync();
        }

        Task IUserService.UpdateUserRoleAsync(string currentUserId, string userId, UpdateUserRoleDto updateUserRoleDto)
        {
            return UpdateUserRoleAsync(currentUserId, userId, updateUserRoleDto);
        }
    }
}
