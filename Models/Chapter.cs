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

        public Comic? Comic { get; set; }

        public List<Page>? Pages { get; set; }

        public List<ReadingHistory>? ReadingHistories { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
