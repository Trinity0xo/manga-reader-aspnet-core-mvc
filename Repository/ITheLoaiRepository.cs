using WEBTRUYEN.Models;
namespace WEBTRUYEN.Repository
{
    public interface ITheLoaiRepository
    {
        Task<IEnumerable<TheLoai>> GetAllAsync();
        Task<TheLoai> GetByIdAsync(int id);
        Task AddAsync(TheLoai theLoai);
        Task UpdateAsync(TheLoai theLoai);
        Task DeleteAsync(int id);
    }
}