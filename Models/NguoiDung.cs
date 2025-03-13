using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace WEBTRUYEN.Models{
    public class NguoiDung : IdentityUser{

        [AllowNull]
        [DisplayName("Tên")]
        public string FirstName { get; set; }

        [AllowNull]
        [DisplayName("Họ")]
        public string LastName { get; set; }

        [AllowNull]
        [DisplayName("Ảnh đại diện")]
        public string? AvatarUrl { get; set; }

        [Required]
        [DisplayName("Tham gia lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}