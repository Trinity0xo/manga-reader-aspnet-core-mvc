using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace MangaReader.Models
{
    public class Role: IdentityRole
    {
        [DisplayName("Tên vai trò")]
        public override string? Name { get; set; }

        [DisplayName("Tên hiển thị")]
        public required string DisplayName { get; set; }

        [DisplayName("Tên chuẩn hóa")]
        public override string? NormalizedName { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
