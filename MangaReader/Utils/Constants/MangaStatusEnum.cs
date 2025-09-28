using System.ComponentModel.DataAnnotations;

namespace MangaReader.Utils.Constants
{
    public enum MangaStatusEnum
    {
        [Display(Name = "Đang tiến hành")]
        OnGoing,

        [Display(Name = "Đã hoàn thành")]
        Completed,

        [Display(Name = "Tạm dừng")]
        Hiatus,

        [Display(Name = "Đã hủy")]
        Cancelled
    }
}
