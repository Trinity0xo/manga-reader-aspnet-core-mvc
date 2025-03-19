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

        public async Task<IEnumerable<Genre>> GetAllAsync(int pageSize, int pageNumber, string searchValue)
        {
            var query = _context.Genres.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(c => c.Name.Contains(searchValue));
            }

            query = query.OrderByDescending(c => c.UpdatedDate).Skip(pageNumber * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Genre>> GetAllNoPaginateAsync()
        {
            return await _context.Genres.OrderBy(c => c.Name).ToListAsync();
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

        public async Task<int> GetTotalCountAsync(string? searchValue = null)
        {
            var query = _context.Genres.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(c => c.Name.Contains(searchValue));
            }

            return await query.CountAsync();
        }

        public async Task UpdateAsync(Genre genre)
        {
            genre.UpdatedDate = DateTime.Now;
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }
    }
}