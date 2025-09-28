using MangaReader.Dto;
using MangaReader.Dto.Chapter;
using MangaReader.Models;
using MangaReader.Repositories;
using MangaReader.Utils;
using MangaReader.ViewModels;

namespace MangaReader.Services.Implementations
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IMangaRepository _mangaRepository;
        private readonly ILogger<ChapterService> _logger;
        private readonly IReadingHistoryRepository _readingHistoryRepository;
        private readonly IFileService _fileService;

        public ChapterService(IChapterRepository chapterRepository, ILogger<ChapterService> logger, IReadingHistoryRepository readingHistoryRepository, IMangaRepository mangaRepository, IFileService fileService)
        {
            _chapterRepository = chapterRepository;
            _logger = logger;
            _readingHistoryRepository = readingHistoryRepository;
            _mangaRepository = mangaRepository;
            _fileService = fileService;
        }

        public async Task<bool> ExistsBySlugAsync(string? chapterSlug)
        {
            return await _chapterRepository.ExistsBySlugAsync(chapterSlug);
        }

        public async Task<Chapter?> GetChapterBySlugAsync(string? chapterSlug)
        {
            return await _chapterRepository.GetBySlugAsync(chapterSlug);
        }

        public async Task<bool> ExistsByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug)
        {
            return await _chapterRepository.ExistsByMangaIdAndChapterSlugAsync(mangaId, chapterSlug);
        }

        public async Task<Chapter?> GetChapterByMangaIdAndChapterSlugAsync(int? mangaId, string? chapterSlug)
        {
            return await _chapterRepository.GetByMangaIdAndChapterSlugAsync(mangaId, chapterSlug);
        }

        public async Task<PageResponse<IEnumerable<Chapter>>> GetNewestChaptersAsync(PageableDto pageableDto)
        {
            return await _chapterRepository.GetAllAsync(pageableDto);
        }

        public async Task<int> CountAllChaptersAsync()
        {
            return await _chapterRepository.CountAllChaptersAsync();
        }

        public async Task<ChapterDetailsViewModel> GetChapterDetailsByIdAsync(int? chapterId, string? userId)
        {
            Chapter? chapter = await _chapterRepository.GetByIdAsync(chapterId);
            if(chapter == null)
            {
                _logger.LogError("Failed to fetch chapter details. Chapter not found with ID: {ChapterId}", chapterId);
                throw new KeyNotFoundException("Không tìm thấy chương truyện");
            }

            if (userId != null)
            {
                ReadingHistory? readingHistory = await _readingHistoryRepository.GetByUserIdAndMangaIdAsync(chapter.MangaId, userId);
                if(readingHistory != null)
                {
                    readingHistory.Chapter = chapter;
                    await _readingHistoryRepository.UpdateAsync(readingHistory);
                }
                else
                {
                    ReadingHistory newReadingHistory = new()
                    {
                        MangaId = chapter.MangaId,
                        ChapterId = chapter.Id,
                        UserId = userId
                    };

                    await _readingHistoryRepository.AddAsync(newReadingHistory);
                }
            }

            IEnumerable<Chapter> chapters = await _chapterRepository.GetAllAsync(chapter.MangaId);
            Chapter? nextChapter = await _chapterRepository.GetNextChapterByMangaIdAndChapterIdAsync(chapter.MangaId, chapter.Id);
            Chapter? previousChapter = await _chapterRepository.GetPreviousChapterByMangaIdAndChapterIdAsync(chapter.MangaId, chapter.Id);
            
            ChapterDetailsViewModel viewModel = new()
            {
                NextChapter = nextChapter,
                PreviousChapter = previousChapter,
                Chapter = chapter,
                Chapters = chapters,
            };

            return viewModel;
        }

        public async Task<Chapter> GetChapterDetailsByIdAsync(int? chapterId)
        {
            Chapter? chapter = await _chapterRepository.GetByIdAsync(chapterId);
            if (chapter == null)
            {
                _logger.LogError("Fail to fetch chapter details. Chapter not found with ID: {ChapterId}", chapterId);
                throw new KeyNotFoundException("Không tìm thấy chương truyện");
            }

            return chapter;
        }

        public async Task<CreateChapterDto> GetInfoForCreateChapterAsync(int? mangaId)
        {
            Manga? manga = await _mangaRepository.GetByIdAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to fetch manga info for creating chapter. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            CreateChapterDto createChapterDto = new()
            {
                MangaId = manga.Id,
                MangaTitle = manga.Title,
                MangaSlug = manga.Slug,
            };

            return createChapterDto;
        }

        public async Task CreateNewChapterAsync(int mangaId, CreateChapterDto createChapterDto)
        {
            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to create new chapter. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            string newChapterSlug = SlugUtils.GenerateSlug(createChapterDto.Title);

            var chapter = new Chapter
            {
                Title = createChapterDto.Title,
                Slug = newChapterSlug,
                Pages = []
            };

            if (createChapterDto.Pages != null && createChapterDto.Pages.Count != 0)
            {
                List<string>? pageFileNames = await _fileService.UploadChapterPagesAsync(createChapterDto.Pages, manga.Slug, newChapterSlug);

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

            manga.Chapters.Add(chapter);

            await _mangaRepository.UpdateAsync(manga);
        }

        public async Task<UpdateChapterDto> GetInfoForUpdateChapterAsync(int? chapterId)
        {
            Chapter? chapter = await _chapterRepository.GetByIdAsync(chapterId);
            if (chapter == null)
            {
                _logger.LogError("Failed to fetch chapter info for update. Chapter not found with ID: {ChapterId}", chapterId);
                throw new KeyNotFoundException("Không tìm thấy chương truyện");
            }

            UpdateChapterDto updateChapterDto = new()
            {
                MangaId = chapter.Manga.Id,
                MangaTitle = chapter.Manga.Title,
                MangaSlug = chapter.Manga.Slug,
                ChapterId = chapter.Id,
                OldTitle = chapter.Title,
                NewTitle = chapter.Title,
                OldPages = chapter.Pages.Select(p => p.Image).ToList(),
            };

            return updateChapterDto;
        }

        public async Task UpdateChapterAsync(int chapterId, UpdateChapterDto updateChapterDto)
        {
            if (chapterId != updateChapterDto.ChapterId)
            {
                _logger.LogError("Failed to update chapter. Chapter ID mismatch. Path ID: {ChapterId}, DTO ID: {DtoChapterId}",
                    chapterId,
                    updateChapterDto.ChapterId
                );
                throw new KeyNotFoundException("Không tìm thấy chương truyện");
            }

            Chapter? chapter = await _chapterRepository.GetByIdAsync(updateChapterDto.ChapterId);
            if (chapter == null)
            {
                _logger.LogError("Failed to update chapter. Chapter not found with ID: {ChapterId}", chapterId);
                throw new KeyNotFoundException("Không tìm thấy chương truyện");
            }

            string oldChapterSlug = chapter.Slug;
            string? newChapterSlug = null;
            if (chapter.Title != updateChapterDto.NewTitle)
            {
                newChapterSlug = SlugUtils.GenerateSlug(updateChapterDto.NewTitle);
            }

            if (updateChapterDto.NewPages != null && updateChapterDto.NewPages.Count > 0)
            {
                List<string> oldPageFileNames = chapter.Pages.Select(p => p.Image).ToList();
                List<string>? pageFileNames = await _fileService.UpdateChapterPagesAsync(updateChapterDto.NewPages, oldPageFileNames, chapter.Manga.Slug, oldChapterSlug);

                if (pageFileNames != null && pageFileNames.Count > 0)
                {
                    chapter.Pages.Clear();

                    foreach (var fileName in pageFileNames)
                    {
                        chapter.Pages.Add(new Page
                        {
                            Image = fileName
                        });
                    }
                }
            }

            if (newChapterSlug != null)
            {
                chapter.Slug = newChapterSlug;
                _fileService.RenameChapterFolder(chapter.Manga.Slug, oldChapterSlug, newChapterSlug);
            }

            chapter.Title = updateChapterDto.NewTitle;

            await _chapterRepository.UpdateAsync(chapter);
        }

        public async Task<Chapter> GetInfoForDeleteChapterAsync(int? chapterId)
        {
            Chapter? chapter = await _chapterRepository.GetByIdAsync(chapterId);
            if (chapter == null)
            {
                _logger.LogError("Failed to fetch chapter info for delete. Chapter not found with ID: {ChapterId}", chapterId);
                throw new KeyNotFoundException("Không tìm thấy chương truyện");
            }

            return chapter;
        }

        public async Task DeleteChapterAsync(int chapterId)
        {
            Chapter? chapter = await _chapterRepository.GetByIdAsync(chapterId);
            if (chapter != null)
            {
                await _readingHistoryRepository.DeleteAllByChapterIdAsync(chapterId);

                _fileService.DeleteChapterPages(chapter.Manga.Slug, chapter.Slug);
                await _chapterRepository.DeleteAsync(chapterId);
            }
        }

        public async Task<MangaChaptersViewModel> GetMangaChaptersAsync(int? mangaId, PageableDto pageableDto)
        {
            Manga? manga = await _mangaRepository.GetByIdWithGenresAsync(mangaId);
            if (manga == null)
            {
                _logger.LogError("Failed to fetch manga chapters. Manga not found with ID: {MangaId}", mangaId);
                throw new KeyNotFoundException("Không tìm thấy truyện");
            }

            PageResponse<IEnumerable<Chapter>> chapters = await _chapterRepository.GetAllAsync(mangaId, pageableDto);

            MangaChaptersViewModel result = new()
            {
                MangaId = manga.Id,
                MangaTitle = manga.AuthorName,
                MangaSlug = manga.Slug,
                Chapers = chapters
            };

            return result;
        }
    }
}
