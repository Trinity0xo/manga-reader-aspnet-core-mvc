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
        public int Id { get; set; }

        [DisplayName("Ảnh bìa")]
        public string? CoverUrl { get; set; }

        [Required(ErrorMessage = "Tên truyện là bắt buộc")]
        [DisplayName("Tên truyện")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tác giá là bắt buộc")]
        [DisplayName("Tác giả")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Ngày xuất bản là bắt buộc")]
        [DisplayName("Ngày xuất bản")]
        public DateTime PublicAt { get; set; }

        [DisplayName("Trạng thái")]
        public string Status { get; set; } = "Đang tiến hành";

        [DisplayName("Thể loại")]
        public List<Genre>? Genres { get; set; }

        [DisplayName("Chương")]
        public List<Chapter>? Chapters { get; set; }

        [DisplayName("người dùng theo dõi")]
        public List<User>? Users { get; set; }

        [DisplayName("Khởi tạo truyện lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisplayName("Cập nhật chương lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
