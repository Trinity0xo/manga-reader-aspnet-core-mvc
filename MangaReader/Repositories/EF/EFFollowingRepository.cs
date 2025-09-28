using MangaReader.Dto;
using MangaReader.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Repositories.EF
{
    public class EFFollowingRepository : IFollowRepository
    {
        private readonly ApplicationDbContext _context;

        public EFFollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Following following)
        {
            _context.Followings.Add(following);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId, int mangaId)
        {
            var following = await _context.Followings.FirstOrDefaultAsync(fo => fo.UserId == userId && fo.MangaId == mangaId);
            if (following != null)
            {
                _context.Followings.Remove(following);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PageResponse<IEnumerable<Following>>> GetAllAsync(string userId, PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.Followings.Where(fo => fo.UserId == userId).AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Include(fo => fo.Manga)
                .ThenInclude(fo => fo.Genres)
                .OrderByDescending(f => f.Manga.Chapters
                    .OrderByDescending(c => c.CreatedAt)
                        .Select(c => c.CreatedAt)
                        .FirstOrDefault())
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PageResponse<IEnumerable<Following>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<bool> ExistsByUserIdAndMangaIdAsync(string userId, int mangaId)
        {
            return await _context.Followings.AnyAsync(fo => fo.UserId == userId && fo.MangaId == mangaId);
        }

        public async Task<int> CountByMangaId(int? mangaId)
        {
            return await _context.Followings.CountAsync(fo => fo.MangaId == mangaId);
        }
    }
}
