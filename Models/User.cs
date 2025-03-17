using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WEBTRUYEN.Models
{
    public class User : IdentityUser
    {
        [DisplayName("Ảnh đại diện")]
        public string? AvatarUrl { get; set; }

        [DisplayName("Truyện đã theo dõi")]
        public List<Comic>? Comics { get; set; }

        [DisplayName("Lịch sử đọc")]
        public List<ReadingHistory>? ReadingHistories { get; set; }

        [DisplayName("Tham gia lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}