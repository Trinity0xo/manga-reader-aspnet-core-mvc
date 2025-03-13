using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public class EFTheLoaiRepository : ITheLoaiRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTheLoaiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TheLoai theLoai)
        {
            _context.TheLoais.Add(theLoai);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var theLoai = await _context.TheLoais.FindAsync(id);
            _context.TheLoais.Remove(theLoai);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TheLoai>> GetAllAsync()
        {
            return await _context.TheLoais.ToListAsync();
        }

        public async Task<TheLoai> GetByIdAsync(int id)
        {
            return await _context.TheLoais.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(TheLoai theLoai)
        {
            _context.TheLoais.Update(theLoai);
            await _context.SaveChangesAsync();
        }
    }
}