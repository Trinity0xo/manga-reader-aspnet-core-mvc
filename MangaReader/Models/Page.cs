using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaReader.Models
{
    public class Page
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Ảnh trang")]
        public required string Image { get; set; }

        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }

        public Chapter Chapter { get; set; } = default!;
    }
}
