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

        public async Task<int> CountFollowersAsync(int id)
        {
            var comic = await _context.Comics
                .Where(x => x.Id == id)
                .Include(c => c.Users)
                .FirstOrDefaultAsync();
            return comic?.Users.Count() ?? 0;
        }

        public async Task DeleteAsync(int id)
        {
            var comic = await _context.Comics.FindAsync(id);
            _context.Comics.Remove(comic);
            await _context.SaveChangesAsync();
        }

        public async Task FollowComicAsync(User userWithComics, Comic comic)
        {
            userWithComics.Comics.Add(comic);
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

        public async Task<IEnumerable<Comic>> GetFollowingAsync(string userId)
        {
            return await _context.Comics
                .Where(c => c.Users.Any(u => u.Id == userId))
                    .OrderByDescending(c => c.Chapters
                        .OrderByDescending(c => c.CreatedDate)
                            .Select(c => c.CreatedDate).FirstOrDefault())
                    .Include(c => c.Chapters.OrderByDescending(ch => ch.CreatedDate))
                .ToListAsync();
        }

        public async Task RemoveFromFollowing(User userWithComics, Comic comic)
        {
            userWithComics.Comics.Remove(comic);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comic comic)
        {
            _context.Comics.Update(comic);
            await _context.SaveChangesAsync();
        }
    }
}
