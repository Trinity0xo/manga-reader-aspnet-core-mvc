using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTRUYEN.Models
{
    public class Page
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ImageUrl {  get; set; }

        [ForeignKey("Chapter")]
        public int ChapterId {  get; set; }
    }
}
