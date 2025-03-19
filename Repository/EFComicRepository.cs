using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public class EFComicRepository : IComicRepository
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

        public async Task<IEnumerable<Comic>> GetAllAsync(int pageSize, int pageNumber, string searchValue, int genreId)
        {
            var query = _context.Comics.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(c => c.Name.Contains(searchValue));
            }

            if (genreId > 0)
            {
                query = query.Where(c => c.Genres.Any(g => g.Id == genreId));
            }

            query = query.Include(c => c.Chapters.OrderByDescending(ch => ch.CreatedDate))
                        .OrderByDescending(c => c.UpdatedDate)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Comic> GetByIdAsync(int id)
        {
            return await _context.Comics.Include(g => g.Genres).Include(c => c.Chapters.OrderByDescending(c => c.CreatedDate)).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetTotalCountAsync(string searchValue, int genreId)
        {
            var query = _context.Comics.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(c => c.Name.Contains(searchValue));
            }

            if (genreId > 0)
            {
                query = query.Where(c => c.Genres.Any(c => c.Id == genreId));

            }

            return await query.CountAsync();
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

        public async Task<bool> IsFollowingAsync(User userWithComics, Comic comic)
        {
            return await Task.FromResult(userWithComics.Comics.Any(c => c.Id == comic.Id));
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
