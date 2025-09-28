using MangaReader.Dto;
using MangaReader.Dto.Manga;
using MangaReader.Models;
using MangaReader.Repositories;
using MangaReader.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Services.Implementations
{
    public class HomeContentService : IHomeContentService
    {
        private readonly IMangaRepository _mangaRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly UserManager<User> _userManager;

        public HomeContentService(IMangaRepository mangaRepository, IChapterRepository chapterRepository, IGenreRepository genreRepository, UserManager<User> userManager)
        {
            _mangaRepository = mangaRepository;
            _chapterRepository = chapterRepository;
            _genreRepository = genreRepository;
            _userManager = userManager;
        }

        public async Task<HomeContentViewModel> GetHomeContentAsync()
        {
            FilterMangaDto filterMangaDto = new()
            {
                Limit = 10,
            };

            PageResponse<IEnumerable<Manga>> newestMangas = await _mangaRepository.GetAllAsync(filterMangaDto);


            PageableDto pageableDto = new()
            {
                Limit = 20
            };


            PageResponse<IEnumerable<Manga>> topViewdMangas = await _mangaRepository.GetMostViewdAsync(pageableDto);

            PageResponse<IEnumerable<Manga>> topFollowedMangas = await _mangaRepository.GetMostFollowedAsync(pageableDto);

            PageResponse<IEnumerable<Manga>> newestChapters = await _mangaRepository.GetNewestChapterAsync(pageableDto);


            return new HomeContentViewModel
            {
                NewestChapterMangas = newestChapters.Data,
                MostViewdMangas = topViewdMangas.Data,
                MostFollowedManga = topFollowedMangas.Data,
                NewestMangas = newestMangas.Data
            };
        }

        public async Task<DashboardContentViewModel> GetDashboardContentAsync()
        {
            PageableDto pageableDto = new();
            PageResponse<IEnumerable<Chapter>> response = await _chapterRepository.GetAllAsync(pageableDto);
            int chapterCount = await _chapterRepository.CountAllChaptersAsync();
            int mangaCount = await _mangaRepository.CountAllMangasAsync();
            int genreCount = await _genreRepository.CountAllGenresAsync();
            int userCount = await _userManager.Users
                            .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "User"))
                            .CountAsync();

            return new DashboardContentViewModel()
            {
                ChapterCount = chapterCount,
                MangaCount = mangaCount,
                GenreCount = genreCount,
                UserCount = userCount,
                NewestChapters = response.Data,
            };
        }
    }
}
