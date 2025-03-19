using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public class EFReadingHistoryRepository : IReadingHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFReadingHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToHistoryAsync(string userId, Chapter chapter)
        {
            var readingHistory = await _context.ReadingHistories
                    .Where(r => r.UserId == userId && r.Chapter.ComicId == chapter.ComicId)
                    .Include(r => r.Chapter)
                    .FirstOrDefaultAsync();

            if (readingHistory != null)
            {
                _context.ReadingHistories.Remove(readingHistory);
            }

            var newReadingHistory = new ReadingHistory
            {
                UserId = userId,
                ChapterId = chapter.Id,
            };

            _context.ReadingHistories.Add(newReadingHistory);
            await _context.SaveChangesAsync();
        }



        public async Task<IEnumerable<ReadingHistory>> GetReadingHistoryAsync(string userId)
        {
            return await _context.ReadingHistories
                .Where(u => u.UserId == userId)
                    .Include(c => c.Chapter)
                        .ThenInclude(c => c.Comic)
                .OrderByDescending(c => c.ReadAt).ToListAsync();
        }

        public async Task RemoveFromHistoryAsync(string userId, Chapter chapter)
        {
            var readingHistory = await _context.ReadingHistories
                .Where(r => r.UserId == userId && r.Chapter.ComicId == chapter.ComicId)
                .Include(r => r.Chapter)
                .FirstOrDefaultAsync();

            if (readingHistory != null)
            {
                _context.ReadingHistories.Remove(readingHistory);
            }

            await _context.SaveChangesAsync();
        }
    }
}