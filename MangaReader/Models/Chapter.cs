using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaReader.Models
{
    public class Chapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Tên chương")]
        public required string Title { get; set; }

        [Required]
        [DisplayName("Slug hiển thị")]
        public required string Slug { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("Manga")]
        public int MangaId { get; set; }

        public Manga Manga { get; set; } = default!;

        public List<Page> Pages { get; set; } = [];
    }
}
