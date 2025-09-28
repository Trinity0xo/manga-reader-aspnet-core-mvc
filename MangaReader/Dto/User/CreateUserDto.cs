using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MangaReader.Dto.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tên phải từ 2 đến 50 ký tự")]
        [DisplayName("Tên")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ là bắt buộc")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Họ phải từ 2 đến 50 ký tự")]
        [DisplayName("Họ")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [DisplayName("Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự và tối đa 100 ký tự")]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu xác nhận là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [DisplayName("Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [DisplayName("Vai trò")]
        public string RoleId { get; set; } = string.Empty;
    }
}
