using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace MangaReader.Models
{
    public class User: IdentityUser
    {
        [DisplayName("Tên")]
        public required string FirstName { get; set; }

        [DisplayName("Họ")]
        public required string LastName { get; set;}

        [DisplayName("Ảnh đại diện")]
        public  string? Avatar { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Following> Followings { get; set; } = [];

        public List<ReadingHistory> ReadingHistories { get; set; } = [];

        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
