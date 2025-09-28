using MangaReader.Services;
using MangaReader.Utils;
using MangaReader.Utils.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MangaReader.Dto.Manga
{
    public class UpdateMangaDto : IValidatableObject
    {
        [Required]
        public required int Id { get; set; }

        [ValidateNever]
        public required string OldCover { get; set; }

        [ValidateNever]
        public required string OldTitle { get; set; }

        [DisplayName("Ảnh bìa")]
        public IFormFile? NewCover { get; set; }

        [Required(ErrorMessage = "Tên truyện là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên truyện phải từ 2 đến 255 ký tự")]
        [DisplayName("Tên truyện")]
        public string NewTitle { get; set; } = string.Empty;

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
        public MangaStatusEnum Status { get; set; }

        [DisplayName("Thể loại")]
        public List<int>? GenreIds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(NewCover != null)
            {
                if (!ImageUtils.IsValidImageType(NewCover.ContentType))
                {
                    yield return new ValidationResult(
                        "File ảnh không hợp lệ",
                        [nameof(NewCover)]
                    );
                }
            }
        }
    }
}
