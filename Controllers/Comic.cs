using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Controllers
{
    public class Comic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã truyện")]
        public int Id { get; set; }

        [DisplayName("Tên truyện")]
        public string Name { get; set; }

        [DisplayName("Thể loại")]
        public List<Genre>? Genres { get; set; }

        [DisplayName("Khởi tạo lúc")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DisplayName("Cập nhật lúc")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
