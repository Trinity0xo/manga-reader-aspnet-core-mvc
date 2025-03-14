using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public class EFGenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public EFGenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _context.Genres.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Genre>?> GetByIdAsync(List<int> genreIds)
        {
            return await _context.Genres
                         .Where(g => genreIds.Contains(g.Id))
                         .ToListAsync();
        }

        public async Task UpdateAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }
    }
}