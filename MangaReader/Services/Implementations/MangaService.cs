using MangaReader.Dto;
using MangaReader.Dto.Manga;
using MangaReader.Models;
using MangaReader.Repositories;
using MangaReader.Utils;
using MangaReader.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace MangaReader.Services.Implementations
{
    public class MangaService : IMangaService
    {
        private readonly IMangaRepository _mangaRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IFileService _fileService;
        private readonly IGenreRepository _genreRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly ILogger<MangaService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IReadingHistoryRepository _readingHistoryRepository;


        public MangaService(IMangaRepository mangaRepository, IFileService fileService, IGenreRepository genreRepository, IChapterRepository chapterRepository, ILogger<MangaService> logger, UserManager<User> userManager, SignInManager<User> signInManager, IFollowRepository followRepository, IReadingHistoryRepository readingHistoryRepository)
        {
            _mangaRepository = mangaRepository;
            _fileService = fileService;
            _genreRepository = genreRepository;
            _chapterRepository = chapterRepository;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _followRepository = followRepository;
            _readingHistoryRepository = readingHistoryRepository;
        }

        public async Task CreateNewMangaAsync(CreateMangaDto createMangaDto)
        {
            string mangaSlug = SlugUtils.GenerateSlug(createMangaDto.Title);
   
            string? coverFileName = await _fileService.UploadMangaCoverAsync(createMangaDto.Cover, mangaSlug);
            if (coverFileName == null) {
                _logger.LogError("Failed to create new manga. Error during upload manga cover");
                throw new InvalidOperationException("Không thể tải ảnh bìa cho truyện");
            }

            Manga newManga = new()
            {
                Cover = coverFileName,
                Title = createMangaDto.Title,
                Slug = mangaSlug,
                Description = createMangaDto.Description,
                AuthorName = createMangaDto.AuthorName,
                PublishedAt = createMangaDto.PublishedAt,
                Status = createMangaDto.Status,
            };

            if (createMangaDto.GenreIds != null && createMangaDto.GenreIds.Count != 0)
            {
                var genres = await _genreRepository.GetByIdsAsync(createMangaDto.GenreIds);

                foreach (var genre in genres)
                {
                    newManga.Genres.Add(genre);
                }
            }

            if (createMangaDto.Chapters != null && createMangaDto.Chapters.Count != 0)
            {
                newManga.Chapters = [];

                foreach (var chapterDto in createMangaDto.Chapters)
                {
                    string chapterSlug = SlugUtils.GenerateSlug(chapterDto.Title);

                    var chapter = new Chapter
                    {
                        Title = chapterDto.Title,
                        Slug = chapterSlug,
                        Pages = []
                    };

                    if (chapterDto.Pages != null && chapterDto.Pages.Count != 0)
                    {
                        List<string>? pageFileNames = await _fileService.UploadChapterPagesAsync(chapterDto.Pages, mangaSlug, chapterSlug);

                        if (pageFileNames != null)
                        {
                            foreach (var fileName in pageFileNames)
                            {
                                chapter.Pages.Add(new Page
                                {
                                    Image = fileName
                                });
                            }
                        }
                    }

                    newManga.Chapters.Add(chapter);
                }
            }

            await _mangaRepository.AddAsync(newManga);
        }

        public async Task DeleteMangaAsync(int mangaId)
        {
            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if (manga != null)
            {
                string mangaFolderName = manga.Slug;
                _fileService.DeleteMangaRelativeFiles(mangaFolderName);
                await _mangaRepository.DeleteAsync(mangaId);
            }
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetAllMangasAsync(FilterMangaDto filterMangaDto)
        {
            return await _mangaRepository.GetAllAsync(filterMangaDto);
        }

        public async Task<UpdateMangaDto> GetMangaInfoForUpdateAsync(int? mangaId)
        {
            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to fetch manga info for create new manga. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            UpdateMangaDto updateMangaDto = new()
            {
                OldTitle = manga.Title,
                NewTitle = manga.Title,
                Description = manga.Description,
                AuthorName = manga.AuthorName,
                OldCover = manga.Cover,
                PublishedAt = manga.PublishedAt,
                Status = manga.Status,
                GenreIds = manga.Genres.Select(g => g.Id).ToList(),
                Id = manga.Id,
            };

            return updateMangaDto;
        }

        public async Task UpdateMangaAsync(int mangaId, UpdateMangaDto updateMangaDto)
        {
            if(mangaId != updateMangaDto.Id)
            {
                _logger.LogError("Failed to update manga. Manga ID mismatch. Path ID: {MangaId}, DTO ID: {DtoId}", mangaId, updateMangaDto.Id);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to update manga. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            string oldMangaSlug = manga.Slug;
            string newMangaSlug = SlugUtils.GenerateSlug(updateMangaDto.NewTitle);

            if (updateMangaDto.NewCover != null)
            {
                string? newCoverFileName = await _fileService.UploadMangaCoverAsync(updateMangaDto.NewCover, oldMangaSlug);
                if (newCoverFileName == null) {
                    _logger.LogError("Failed to upload manga cover during update manga with ID: {MangaId}", mangaId);
                    throw new InvalidOperationException("Không thể tải ảnh bìa cho truyện");
                }
               
                if (!string.IsNullOrEmpty(manga.Cover))
                {
                    _fileService.DeleteMangaCover(manga.Cover, oldMangaSlug);
                }

                manga.Cover = newCoverFileName;
            }

            if (!string.Equals(oldMangaSlug, newMangaSlug, StringComparison.OrdinalIgnoreCase))
            {
                _fileService.RenameMangaFolder(oldMangaSlug, newMangaSlug);
                manga.Slug = newMangaSlug;
            }

            manga.Title = updateMangaDto.NewTitle;
            manga.Description = updateMangaDto.Description;
            manga.AuthorName = updateMangaDto.AuthorName;
            manga.PublishedAt = updateMangaDto.PublishedAt;
            manga.Status = updateMangaDto.Status;


            List<Genre> newGenres = [];

            if (updateMangaDto.GenreIds != null) {
                IEnumerable<Genre> genres = await _genreRepository.GetByIdsAsync(updateMangaDto.GenreIds);
                newGenres = genres.ToList();
            }

            manga.Genres.Clear();
            manga.Genres = newGenres;

            await _mangaRepository.UpdateAsync(manga);
        }

        public async Task<Manga> GetMangaDetailsByIdAsync(int? mangaId)
        {
            int chapterLimit = 15;
            Manga? manga = await _mangaRepository.GetByIdAsync(mangaId, chapterLimit);
            if (manga == null)
            {
                _logger.LogError("Failed to fetch manga details. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            return manga;
        }

        public async Task<Manga> GetMangaInfoForDeleteAsync(int? mangaId)
        {
            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to fetch manga for delete. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            return manga;
        }

        public async Task<bool> ExistsByTitleAsync(string? title)
        {
            return await _mangaRepository.ExistsByTitleAsync(title);
        }

        public async Task<Manga?> GetMangaBySlugAsync(string? slug)
        {
            return await _mangaRepository.GetBySlugAsync(slug);
        }

        public async Task<int> CountAllMangasAsync()
        {
            return await _mangaRepository.CountAllMangasAsync();
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetMostViewedMangasAsync(FilterMangaDto filterMangaDto)
        {
            return await _mangaRepository.GetMostViewdAsync(filterMangaDto);
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetMostFollowedMangasAsync(FilterMangaDto filterMangaDto)
        {
            return await _mangaRepository.GetMostFollowedAsync(filterMangaDto);
        }

        public async Task<MangaDetailsViewModel> GetMangaDetailsByIdAsync(int? mangaId, string? userId)
        {
            Manga? manga = await _mangaRepository.GetByIdWithChaptersAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to fetch manga details. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            bool isFollowed = false;

            if (userId != null)
            {
                isFollowed = await _followRepository.ExistsByUserIdAndMangaIdAsync(userId, manga.Id);
            }

            int followedCount = await _followRepository.CountByMangaId(mangaId);
            int viewedCount = await _readingHistoryRepository.CountByMangaId(mangaId);

            MangaDetailsViewModel viewModel = new()
            {
                Manga = manga,
                IsFollowed = isFollowed,
                TotalFollowed = followedCount,
                TotalViewed = viewedCount,
            };
           
            return viewModel;
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetMostFollowedMangasAsync(PageableDto pageableDto)
        {
            return await _mangaRepository.GetMostFollowedAsync(pageableDto);
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetMostViewedMangasAsync(PageableDto pageableDto)
        {
            return await _mangaRepository.GetMostViewdAsync(pageableDto);
        }

        public async Task<PageResponse<IEnumerable<Manga>>> GetNewestChapterMangasAsync(PageableDto pageableDto)
        {
            return await _mangaRepository.GetNewestChapterAsync(pageableDto);
        }
    }
}
