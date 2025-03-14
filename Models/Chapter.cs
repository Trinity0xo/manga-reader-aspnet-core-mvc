using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;

namespace WEBTRUYEN.Models
{
    public class Chapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã chương")]
        public int Id { get; set; }

        [DisplayName("Tên chương")]
        public string Name { get; set; }

        [ForeignKey("Comic")]
        public int ComicId { get; set; }

        [DisplayName("Trang truyện")]
        public List<Page>? Pages { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
