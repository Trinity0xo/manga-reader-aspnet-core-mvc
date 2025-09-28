using MangaReader.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MangaReader.Dto.Chapter
{
    public class UpdateChapterDto : IValidatableObject
    {
        [Required]
        public required int MangaId { get; set; }

        [Required]
        public required int ChapterId { get; set; }

        [ValidateNever]
        public required string MangaTitle { get; set; }

        [ValidateNever]
        public required string MangaSlug { get; set; }

        [ValidateNever]
        public required string OldTitle { get; set; }

        [ValidateNever]
        public required List<string> OldPages { get; set; }

        [Required(ErrorMessage = "Tên chương là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên truyện phải từ 2 đến 255 ký tự")]
        [DisplayName("Tên chương")]
        public string NewTitle { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "Chương phải có ít nhất một trang")]
        [DisplayName("Trang truyện")]
        public List<IFormFile>? NewPages { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPages != null && NewPages.Any(page => !ImageUtils.IsValidImageType(page.ContentType)))
            {
                yield return new ValidationResult(
                    "Chương chứa file ảnh không hợp lệ",
                    [nameof(NewPages)]
                );
            }
        }
    }
}
