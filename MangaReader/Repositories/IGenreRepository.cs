using MangaReader.Dto;
using MangaReader.Models;

namespace MangaReader.Repositories
{
    public interface IGenreRepository
    {
        Task<PageResponse<IEnumerable<Genre>>> GetAllAsync(PageableDto pageableDto);
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int? genreId);
        Task<IEnumerable<Genre>> GetByIdsAsync(List<int>? genreIds);
        Task<Genre?> GetBySlugAsync(string? genreSlug);
        Task AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(int genreId);
        Task<bool> ExistsByIdAsync(int? genreId);
        Task<bool> ExistsBySlugAsync(string? genreSlug);
        Task<int> CountAllGenresAsync();
    }
}
