using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaReader.Models
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        [DisplayName("Tên")]
        public required string Name { get; set; }

        [Required]
        [DisplayName("Slug")]
        public required string Slug { get; set; }

        [DisplayName("Mô tả")]
        public string? Description { get; set; }


        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Manga> Mangas { get; set; } = [];
    }
}
