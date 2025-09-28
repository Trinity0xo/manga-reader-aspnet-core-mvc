using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MangaReader.Dto.User
{
    public class UpdateUserRoleDto
    {
        [Required]
        public required string Id { get; set; }

        [ValidateNever]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [DisplayName("Vai trò")]
        public string RoleId { get; set; } = string.Empty;
    }
}
