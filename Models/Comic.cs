using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;

namespace WEBTRUYEN.Models
{
    public class Comic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã truyện")]
        public int Id { get; set; }

        [DisplayName("Ảnh bìa")]
        public string? CoverUrl { get; set; }

        [DisplayName("Tên truyện")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Tác giả")]
        public string Author { get; set; }

        [DisplayName("Ngày xuất bản")]
        public DateTime PublicAt { get; set; }

        [DisplayName("Trạng thái")]
        public string Status { get; set; } = "Đang tiến hành";

        [DisplayName("Thể loại")]
        public List<Genre>? Genres { get; set; }

        [DisplayName("Chương")]
        public List<Chapter>? Chapters { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
