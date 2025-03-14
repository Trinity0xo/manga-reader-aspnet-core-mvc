using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public class EFPageRepository : IPageRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Page page)
        {
            _context.Pages.Add(page);
            await _context.SaveChangesAsync();
        }

        public async Task AddBulkAsync(List<Page> pages)
        {
            _context.Pages.AddRange(pages);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(List<Page> pages)
        {
            _context.Pages.RemoveRange(pages);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Page>> GetAllAsync()
        {
            return await _context.Pages.ToListAsync();
        }

        public async Task<Page> GetByIdAsync(int id)
        {
            return await _context.Pages.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Page page)
        {
            _context.Pages.Update(page);
            await _context.SaveChangesAsync();
        }
    }
}
