using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MangaReader.Utils;
using MangaReader.Utils.Constants;

namespace MangaReader.Dto.Manga
{
    public class CreateMangaDto : IValidatableObject
    {
        [Required(ErrorMessage = "Ảnh bìa là bắt buộc")]
        [DisplayName("Ảnh bìa")]
        public IFormFile Cover { get; set; } = default!;

        [Required(ErrorMessage = "Tên truyện là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên truyện phải từ 2 đến 255 ký tự")]
        [DisplayName("Tên truyện")]
        public string Title { get; set; } = string.Empty;

        [StringLength(5000, MinimumLength = 2, ErrorMessage = "Mô tả phải từ 2 đến 5000 ký tự")]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Tên tác giả là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên tác giả phải từ 2 đến 255 ký tự")]
        [DisplayName("Tác giả")]
        public string AuthorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày xuất bản là bắt buộc")]
        [DisplayName("Ngày xuất bản")]
        public DateTime PublishedAt { get; set; }

        [Required(ErrorMessage = "Trạng thái truyện là bắt buộc")]
        [EnumDataType(typeof(MangaStatusEnum), ErrorMessage = "Trạng thái truyện không hợp lệ")]
        [DisplayName("Trạng thái")]
        public MangaStatusEnum Status { get; set; } = MangaStatusEnum.OnGoing;

        [DisplayName("Thể loại")]
        public List<int> GenreIds { get; set; } = [];

        public List<ChapterDto> Chapters { get; set; } = [];

        public class ChapterDto
        {
            [Required(ErrorMessage = "Tên chương là bắt buộc")]
            [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên truyện phải từ 2 đến 255 ký tự")]
            [DisplayName("Tên chương")]
            public string Title { get; set; } = string.Empty;

            [MinLength(1, ErrorMessage = "Chương phải có ít nhất một trang")]
            [DisplayName("Trang truyện")]
            public List<IFormFile> Pages { get; set; } = [];
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ImageUtils.IsValidImageType(Cover.ContentType))
            {
                yield return new ValidationResult(
                    "File ảnh không hợp lệ",
                    [nameof(Cover)]
                );
            }

            if (Chapters.Count != 0)
            {
                for (int i = 0; i < Chapters.Count; i++)
                {
                    var chapter = Chapters[i];

                    if (chapter.Pages != null && chapter.Pages.Any(p => !ImageUtils.IsValidImageType(p.ContentType)))
                    {
                        yield return new ValidationResult(
                            $"Chương {i + 1} chứa file ảnh không hợp lệ",
                            [$"Chapters[{i}].Pages"]
                        );
                    }
                }
            }
        }

    }
}
