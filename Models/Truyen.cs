using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTRUYEN.Models
{
    public class Truyen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã truyện")]
        public int Id { get; set; }

        [DisplayName("Tên truyện")]
        public string TenTruyen { get; set; }

        [DisplayName("Tác giả")]
        public string TacGia { get; set; }
    }
}