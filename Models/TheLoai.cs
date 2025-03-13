using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WEBTRUYEN.Models
{
    public class TheLoai
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã thể loại")]
        public int Id { get; set; }

        [AllowNull]
        [DisplayName("Tên thể lọai")]
        public string Name { get; set; }

        [AllowNull]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    }
}