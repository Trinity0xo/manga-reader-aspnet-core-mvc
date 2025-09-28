using MangaReader.Dto;
using MangaReader.Dto.Genre;
using MangaReader.Models;

namespace MangaReader.Services
{
    public interface IGenreService
    {
        Task<PageResponse<IEnumerable<Genre>>> GetAllGenresAsync(PageableDto pageableDto);
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<Genre> GetGenreDetailsByIdAsync(int? genreId);
        Task CreateNewGenreAsync(CreateGenreDto createGenreDto);
        Task UpdateGenreAsync(UpdateGenreDto updateGenreDto);
        Task DeleteGenreAsync(int genreId);
        Task<bool> ExistsBySlugAsync(string? slug);
        Task<Genre?> GetGenreBySlugAsync(string? slug);
        Task<UpdateGenreDto> GetInfoForUpdateGenreAsync(int? genreId);
        Task<int> CountAllGenresAsync();
    }
}
