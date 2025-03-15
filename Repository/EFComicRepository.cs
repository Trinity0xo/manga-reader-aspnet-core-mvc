using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public class EFComicRepository:IComicRepository
    {
        private readonly ApplicationDbContext _context;

        public EFComicRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task AddAsync(Comic comic)
        {
            _context.Comics.Add(comic);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comic = await _context.Comics.FindAsync(id);
            _context.Comics.Remove(comic);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comic>> GetAllAsync()
        {
            return await _context.Comics.Include(c => c.Chapters).ToListAsync();
        }

        public async Task<Comic> GetByIdAsync(int id)
        {
            return await _context.Comics.Include(g => g.Genres).Include(c => c.Chapters).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Comic comic)
        {
            _context.Comics.Update(comic);
            await _context.SaveChangesAsync();
        }
    }
}
