using Humanizer.Localisation;
using MangaReader.Dto;
using MangaReader.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Repositories.EF
{
    public class EFReadingHistoryRepository : IReadingHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFReadingHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResponse<IEnumerable<ReadingHistory>>> GetAllAsync(string userId, PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.ReadingHistories.Where(rh => rh.UserId == userId).AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Include(rh => rh.Chapter)
                .Include(rh => rh.Manga)
                .OrderByDescending(rh => rh.UpdatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PageResponse<IEnumerable<ReadingHistory>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task AddAsync(ReadingHistory readingHistory)
        {
            _context.ReadingHistories.Add(readingHistory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReadingHistory readingHistory)
        {
            readingHistory.UpdatedAt = DateTime.Now;
            _context.ReadingHistories.Update(readingHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadingHistory?> GetByUserIdAndMangaIdAsync(int? mangaId, string? userId)
        {
            return await _context.ReadingHistories.SingleOrDefaultAsync(rh => rh.MangaId == mangaId && rh.UserId == userId);
        }

        public async Task DeleteAllByChapterIdAsync(int chapterId)
        {
            var readingHistories = await _context.ReadingHistories.Where(rh => rh.ChapterId == chapterId).ToListAsync();
            if (readingHistories.Count != 0) {
                _context.ReadingHistories.RemoveRange(readingHistories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountByMangaId(int? mangaId)
        {
            return await _context.ReadingHistories.CountAsync(rh => rh.MangaId == mangaId);
        }
    }
}
