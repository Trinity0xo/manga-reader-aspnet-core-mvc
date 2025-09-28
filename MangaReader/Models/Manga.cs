using MangaReader.Utils.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaReader.Models
{
    public class Manga
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Ảnh bìa")]
        public required string Cover { get; set; }

        [Required]
        [DisplayName("Tên truyện")]
        public required string Title { get; set; }

        [Required]
        [DisplayName("Slug")]
        public required string Slug { get; set; }

        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [Required]
        [DisplayName("Tác giả")]
        public required string AuthorName { get; set; }

        [Required]
        [DisplayName("Ngày xuất bản")]
        public DateTime PublishedAt { get; set; }

        [Required]
        [DisplayName("Trạng thái")]
        public MangaStatusEnum Status { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Following> Followings { get; set; } = [];

        public List<ReadingHistory> ReadingHistories { get; set; } = [];

        public List<Chapter> Chapters { get; set; } = [];

        [DisplayName("Thể loại")]
        public List<Genre> Genres { get; set; } = [];
    }
}
