using MangaReader.Dto;
using MangaReader.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Repositories.EF
{
    public class EFChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context;

        public EFChapterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int chapterId)
        {
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PageResponse<IEnumerable<Chapter>>> GetAllAsync(int? mantaId, PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.Chapters.Where(c => c.MangaId == mantaId).AsQueryable();
             
            if (!string.IsNullOrEmpty(pageableDto.Search))
            {
                string keyword = pageableDto.Search.ToLower();
                query = query.Where(c => c.Title.ToLower().Contains(keyword));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Include(c => c.Manga)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PageResponse<IEnumerable<Chapter>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<Chapter?> GetByIdAsync(int? chapterId)
        {
            return await _context.Chapters
                .Include(c => c.Pages)
                .Include(c => c.Manga)
                .SingleOrDefaultAsync(c => c.Id == chapterId);
        }

        public async Task<Chapter?> GetBySlugAsync(string? chapterSlug)
        {
            return await _context.Chapters.SingleOrDefaultAsync(c => c.Slug == chapterSlug);
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            chapter.UpdatedAt = DateTime.Now;
            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();
        }

        public async Task<Chapter?> GetByMangaIdAndChapterId(int? mangaId, int? chapterId)
        {
           return await _context.Chapters
                .Include(c => c.Manga)
                .Include(c=>c.Pages)
                .SingleOrDefaultAsync(c => c.MangaId == mangaId && c.Id == chapterId); ;
        }

        public async Task<bool> ExistsById(int? chapterId)
        {
            return await _context.Chapters.AnyAsync(c => c.Id == chapterId);
        }

        public async Task<bool> ExistsBySlugAsync(string? chapterSlug)
        {
            return await _context.Chapters.AnyAsync(c => c.Slug == chapterSlug);
        }

        public async Task<Chapter?> GetByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug)
        {
            return await _context.Chapters.SingleOrDefaultAsync(c => c.Slug == chapterSlug && c.MangaId == mangaId); 
        }

        public async Task<bool> ExistsByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug)
        {
            return await _context.Chapters.AnyAsync(c => c.Slug == chapterSlug && c.MangaId == mangaId);
        }

        public async Task<PageResponse<IEnumerable<Chapter>>> GetAllAsync(PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.Chapters.AsQueryable();

            if (!string.IsNullOrEmpty(pageableDto.Search))
            {
                string keyword = pageableDto.Search.ToLower();
                query = query.Where(c => c.Title.ToLower().Contains(keyword)
                                      || c.Manga.Title.ToLower().Contains(keyword));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.Manga)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PageResponse<IEnumerable<Chapter>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<int> CountAllChaptersAsync()
        {
            return await _context.Chapters.CountAsync();
        }

        public async Task<IEnumerable<Chapter>> GetAllAsync(int? mangaId)
        {
            return await _context.Chapters.ToListAsync();
        }

        public async Task<Chapter?> GetPreviousChapterByMangaIdAndChapterIdAsync(int? mangaId, int? ChapterId)
        {
            return await _context.Chapters
                .Where(c => c.MangaId == mangaId && c.Id < ChapterId)
                .FirstOrDefaultAsync();
        }

        public async Task<Chapter?> GetNextChapterByMangaIdAndChapterIdAsync(int? mangaId, int? ChapterId)
        {
            return await _context.Chapters
                .Where(c => c.MangaId == mangaId && c.Id > ChapterId)
                .FirstOrDefaultAsync();
        }
    }
}
