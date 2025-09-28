using MangaReader.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Repositories.EF
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
            if (pages == null || pages.Count == 0) return;

            await _context.Pages.AddRangeAsync(pages);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(List<Page> pages)
        {
            if (pages == null || pages.Count == 0) return;

            _context.Pages.RemoveRange(pages);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int pageId)
        {
            var page = await _context.Pages.FindAsync(pageId);
            if (page != null)
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<Page>> GetAllAsync(int chapterId)
        {
            throw new NotImplementedException();
        }

        public async Task<Page?> GetByIdAsync(int pageId)
        {
            return await _context.Pages.SingleOrDefaultAsync(p => p.Id == pageId);
        }

        public async Task UpdateAsync(Page page)
        {
            _context.Pages.Update(page);
            await _context.SaveChangesAsync();
        }
    }
}
