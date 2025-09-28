using MangaReader.Dto;
using MangaReader.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Repositories.EF
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

        public async Task DeleteAsync(int genreId)
        {
            var genre = await _context.Genres.FindAsync(genreId);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PageResponse<IEnumerable<Genre>>> GetAllAsync(PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.Genres.AsQueryable();

            if (!string.IsNullOrEmpty(pageableDto.Search))
            {
                string keyword = pageableDto.Search.ToLower();
                query = query.Where(g => g.Name.ToLower().Contains(keyword));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PageResponse<IEnumerable<Genre>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<Genre?> GetByIdAsync(int? genreId)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == genreId);
        }

        public async Task<Genre?> GetBySlugAsync(string? genreSlug)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Slug == genreSlug);
        }

        public async Task UpdateAsync(Genre genre)
        {
            genre.UpdatedAt = DateTime.Now;
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<IEnumerable<Genre>> GetByIdsAsync(List<int>? genreIds)
        {
            if(genreIds != null)
            {
                return await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
            }
            else
            {
                return [];
            }
        }

        public async Task<bool> ExistsByIdAsync(int? genreId)
        {
            return await _context.Genres.AnyAsync(g => g.Id == genreId);
        }

        public async Task<bool> ExistsBySlugAsync(string? genreSlug)
        {
            return await _context.Genres.AnyAsync(g => g.Slug == genreSlug);
        }

        public async Task<int> CountAllGenresAsync()
        {
            return await _context.Genres.CountAsync();
        }
    }
}
