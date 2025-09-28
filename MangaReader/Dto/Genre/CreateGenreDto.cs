using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MangaReader.Dto.Genre
{
    public class CreateGenreDto
    {
        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên thể loại phải từ 2 đến 255 ký tự")]
        [DisplayName("Tên")]
        public string Name { get; set; } = string.Empty;

        [StringLength(5000, MinimumLength = 2, ErrorMessage = "Mô tả phải từ 2 đến 5000 ký tự")]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }
    }

}
