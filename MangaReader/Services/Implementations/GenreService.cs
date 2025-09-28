using MangaReader.Dto;
using MangaReader.Dto.Genre;
using MangaReader.Models;
using MangaReader.Repositories;
using MangaReader.Utils;

namespace MangaReader.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<GenreService> _logger;

        public GenreService(IGenreRepository genreRepository, ILogger<GenreService> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task CreateNewGenreAsync(CreateGenreDto createGenreDto)
        {
            Genre newGenre = new()
            {
                Name = createGenreDto.Name,
                Slug = SlugUtils.GenerateSlug(createGenreDto.Name),
                Description = createGenreDto.Description,
            };

            await _genreRepository.AddAsync(newGenre);
        }

        public async Task DeleteGenreAsync(int genreId)
        {
            await _genreRepository.DeleteAsync(genreId);
        }

        public async Task<PageResponse<IEnumerable<Genre>>> GetAllGenresAsync(PageableDto pageableDto)
        {
            return await _genreRepository.GetAllAsync(pageableDto);
        }

        public async Task<Genre> GetGenreDetailsByIdAsync(int? genreId)
        {
            Genre? genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre == null)
            {
                _logger.LogError("Failed to get genre details. Genre not found with ID: {GenreId}", genreId);
                throw new KeyNotFoundException("Không tìm thấy thể loại");
            }

            return genre;
        }

        public async Task UpdateGenreAsync(UpdateGenreDto updateGenreDto)
        {
            Genre? genre = await _genreRepository.GetByIdAsync(updateGenreDto.Id);
            if (genre == null)
            {
                _logger.LogError("Failed to update genre. Genre not found with ID: {GenreId}", updateGenreDto.Id);
                throw new KeyNotFoundException($"Không tìm thấy thể loại");
            }

            genre.Name = updateGenreDto.NewName;
            genre.Slug = SlugUtils.GenerateSlug(updateGenreDto.NewName);
            genre.Description = updateGenreDto.Description;

            await _genreRepository.UpdateAsync(genre);
        }

        public async Task<bool> ExistsBySlugAsync(string? slug)
        {
            return await _genreRepository.ExistsBySlugAsync(slug);
        }

        public async Task<Genre?> GetGenreBySlugAsync(string? slug)
        {
            return await _genreRepository.GetBySlugAsync(slug);
        }

        public async Task<UpdateGenreDto> GetInfoForUpdateGenreAsync(int? genreId)
        {
            Genre? genre = await _genreRepository.GetByIdAsync(genreId);
            if(genre == null)
            {
                _logger.LogError("Failed to fetch genre info for update. Genre not found with ID: {GenreId}", genreId);
                throw new KeyNotFoundException($"Không tìm thấy thể loại");
            }

            UpdateGenreDto updateGenreDto = new()
            {
                Id = genre.Id,
                NewName = genre.Name,
                OldName = genre.Name,
                Description = genre.Description
            };

            return updateGenreDto;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllAsync();
        }

        public async Task<int> CountAllGenresAsync()
        {
            return await _genreRepository.CountAllGenresAsync();
        }
    }
}
