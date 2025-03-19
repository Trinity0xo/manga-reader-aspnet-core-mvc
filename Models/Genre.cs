using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTRUYEN.Models
{
    public class Genre
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        [DisplayName("Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tên mô tả là bắt buộc")]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        public List<Comic>? Comics { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

    }
}