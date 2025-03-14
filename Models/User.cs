using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace WEBTRUYEN.Models{
    public class User : IdentityUser{
        [DisplayName("Ảnh đại diện")]
        public string? AvatarUrl { get; set; }

        [DisplayName("Tham gia lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}