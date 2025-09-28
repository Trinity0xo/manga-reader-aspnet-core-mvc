using MangaReader.Dto;
using MangaReader.Dto.Manga;
using MangaReader.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Repositories.EF
{
    public class EFMangaRepository: IMangaRepository
    {
        private readonly ApplicationDbContext _context;

        public EFMangaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task AddAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int mangaId)
        {
            var manga = await _context.Mangas.FindAsync(mangaId);
            if(manga != null)
            {
                _context.Mangas.Remove(manga);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetAllAsync(FilterMangaDto filterMangaDto)
        {
            int page = filterMangaDto.Page;
            int limit = filterMangaDto.Limit;

            var query = _context.Mangas.AsQueryable();

            if (!string.IsNullOrEmpty(filterMangaDto.Search))
            {
                string keyword = filterMangaDto.Search.ToLower();
                query = query.Where(m => m.Title.ToLower().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(filterMangaDto.AuthorName))
            {
                string keyword = filterMangaDto.AuthorName.ToLower();
                query = query.Where(m => m.AuthorName.ToLower().Contains(keyword));
            } 

            if (filterMangaDto.Genres != null && filterMangaDto.Genres.Count != 0)
            {
                query = query.Where(m => m.Genres.Any(g => filterMangaDto.Genres.Contains(g.Slug)));
            }

            if (filterMangaDto.Status != null && filterMangaDto.Status.Count != 0) {
                query = query.Where(m => filterMangaDto.Status.Contains(m.Status));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .OrderByDescending(m => m.CreatedAt)
                .Include(m => m.Genres)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PageResponse<IEnumerable<Manga>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<Manga?> GetByIdAsync(int? mangaId, int chaptersLimit)
        {
            var manga = await _context.Mangas
                .Where(m => m.Id == mangaId)
                .Select(m => new Manga
                {
                    Id = m.Id,
                    Title = m.Title,
                    Cover = m.Cover,
                    Slug = m.Slug,
                    Description = m.Description,
                    AuthorName = m.AuthorName,
                    PublishedAt = m.PublishedAt,
                    Status = m.Status,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    Genres = m.Genres.ToList(),
                    Chapters = m.Chapters
                                .OrderByDescending(c => c.CreatedAt)
                                .Take(chaptersLimit)
                                .AsEnumerable()
                                .ToList()
                })
                .SingleOrDefaultAsync();

            return manga;
        }

        public async Task UpdateAsync(Manga manga)
        {
            manga.UpdatedAt = DateTime.Now;
            _context.Mangas.Update(manga);
            await _context.SaveChangesAsync();
        }

        public async Task<Manga?> GetBySlugAsync(string? mangaSlug)
        {
            return await _context.Mangas.SingleOrDefaultAsync(m => m.Slug == mangaSlug);
        }

        public async Task<Manga?> GetByIdAsync(int? mangaId)
        {
            return await _context.Mangas.SingleOrDefaultAsync(m => m.Id == mangaId);
        }

        public async Task<Manga?> GetByIdWithGenresAsync(int? mangaId)
        {
            return await _context.Mangas.Include(m => m.Genres).SingleOrDefaultAsync(m => m.Id == mangaId);
        }

        public async Task<Manga?> GetByIdWithChaptersAsync(int? mangaId)
        {
            return await _context.Mangas
                .Include(m => m.Genres)
                .Include(m => m.Chapters
                    .OrderByDescending(c => c.CreatedAt))
                .SingleOrDefaultAsync(m => m.Id == mangaId);
        }

        public async Task<bool> ExistsByIdAsync(int? mangaId)
        {
            return await _context.Mangas.AnyAsync(m => m.Id == mangaId);
        }

        public async Task<bool> ExistsByTitleAsync(string? mangaTitle)
        {
            return await _context.Mangas.AnyAsync(m => m.Title == mangaTitle);
        }

        public async Task<int> CountAllMangasAsync()
        {
            return await _context.Mangas.CountAsync();
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetMostViewdAsync(PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.ReadingHistories.AsQueryable();


            //if (!string.IsNullOrEmpty(filterMangaDto.Search))
            //{
            //    string keyword = filterMangaDto.Search.ToLower();
            //    query = query.Where(rh => rh.Manga.Title.ToLower().Contains(keyword));
            //}

            //if (filterMangaDto.Genres?.Count > 0)
            //{
            //    query = query.Where(rh => rh.Manga.Genres.Any(g => filterMangaDto.Genres.Contains(g.Slug)));
            //}

            //if (filterMangaDto.Status?.Count > 0)
            //{
            //    query = query.Where(rh => filterMangaDto.Status.Contains(rh.Manga.Status));
            //}

            var groupedQuery = query
               .GroupBy(rh => rh.MangaId)
               .OrderByDescending(g => g.Count())
               .Select(g => g.Key);

            var totalItems = await groupedQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var topMangaIds = await groupedQuery
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var topViewedMangas = await _context.Mangas
                .Where(m => topMangaIds.Contains(m.Id))
                .Include(m => m.Genres)
                .ToListAsync();

            var items = topMangaIds.Select(id => topViewedMangas.First(m => m.Id == id)).ToList();

            return new PageResponse<IEnumerable<Manga>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = items
            };
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetMostFollowedAsync(PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.Followings.AsQueryable();


            //if (!string.IsNullOrEmpty(filterMangaDto.Search))
            //{
            //    string keyword = filterMangaDto.Search.ToLower();
            //    query = query.Where(fo => fo.Manga.Title.ToLower().Contains(keyword));
            //}

            //if (filterMangaDto.Genres?.Count > 0)
            //{
            //    query = query.Where(fo => fo.Manga.Genres.Any(g => filterMangaDto.Genres.Contains(g.Slug)));
            //}

            //if (filterMangaDto.Status?.Count > 0)
            //{
            //    query = query.Where(fo => filterMangaDto.Status.Contains(fo.Manga.Status));
            //}

            var groupedQuery = query
               .GroupBy(fo => fo.MangaId)
               .OrderByDescending(g => g.Count())
               .Select(g => g.Key);

            var totalItems = await groupedQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var topMangaIds = await groupedQuery
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var topFollowedMangas = await _context.Mangas
                .Where(m => topMangaIds.Contains(m.Id))
                .Include(m => m.Genres)
                .ToListAsync();

            var topMangasOrdered = topMangaIds.Select(id => topFollowedMangas.First(m => m.Id == id)).ToList();

            return new PageResponse<IEnumerable<Manga>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = topMangasOrdered
            };
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetNewestChapterAsync(PageableDto pageableDto)
        {
            int page = pageableDto.Page;
            int limit = pageableDto.Limit;

            var query = _context.Mangas.Where(m => m.Chapters.Any()).Include(m => m.Genres).AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Select(m => new
                {
                    Manga = m,
                    LatestChapter = m.Chapters
                        .OrderByDescending(c => c.CreatedAt)
                        .First()
                })
                .OrderByDescending(x => x.LatestChapter.CreatedAt) 
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var result = items.Select(x => new Manga
            {
                Id = x.Manga.Id,
                Title = x.Manga.Title,
                Slug = x.Manga.Slug,
                Cover = x.Manga.Cover,
                AuthorName = x.Manga.AuthorName,
                PublishedAt = x.Manga.PublishedAt,
                Description = x.Manga.Description,
                Status = x.Manga.Status,
                CreatedAt = x.Manga.CreatedAt,
                UpdatedAt = x.Manga.UpdatedAt,
                Genres = x.Manga.Genres,
                Chapters = new List<Chapter> { x.LatestChapter }
            }).ToList();

            return new PageResponse<IEnumerable<Manga>>
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Data = result
            };
        }
    }
}
