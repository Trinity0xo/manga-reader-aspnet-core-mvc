using MangaReader.Dto;
using MangaReader.Dto.Manga;
using MangaReader.Models;
using MangaReader.Services;
using MangaReader.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Controllers
{
    public class MangasController : Controller
    {
        private readonly IMangaService _mangaService;
        private readonly IGenreService _genreService;
        private readonly UserManager<User> _userManager;

        public MangasController(IMangaService mangaService, IGenreService genreService, UserManager<User> userManager)
        {
            _mangaService = mangaService;
            _genreService = genreService;
            _userManager = userManager;
        }

        // GET: Mangas
        public async Task<IActionResult> Index(FilterMangaDto filterMangaDto)
        {
            PageResponse<IEnumerable<Manga>> mangas = await _mangaService.GetAllMangasAsync(filterMangaDto);
            IEnumerable<Genre> genres = await _genreService.GetAllGenresAsync();
            ViewBag.Genres = genres;
            ViewBag.FilterMangaDto = filterMangaDto;

            return View(mangas);
        }

        // GET: Mangas/MostViewed
        public async Task<IActionResult> MostViewed(PageableDto pageableDto)
        {
            PageResponse<IEnumerable<Manga>> mangas = await _mangaService.GetMostViewedMangasAsync(pageableDto);
            ViewBag.PageableDto = pageableDto;

            return View(mangas);
        }


        // GET: Mangas/MostFollowed
        public async Task<IActionResult> MostFollowed(PageableDto pageableDto)
        {
            PageResponse<IEnumerable<Manga>> mangas = await _mangaService.GetMostFollowedMangasAsync(pageableDto);
            ViewBag.PageableDto = pageableDto;

            return View(mangas);
        }

        // GET: Mangas/NewestChapter
        public async Task<IActionResult> NewestChapter(PageableDto pageableDto)
        {
            PageResponse<IEnumerable<Manga>> mangas = await _mangaService.GetNewestChapterMangasAsync(pageableDto);
            ViewBag.PageableDto = pageableDto;

            return View(mangas);
        }

        // GET: Mangas/manga-05
        [HttpGet("Mangas/{mangaSlug}-{mangaId}")]
        public async Task<IActionResult> Details(string? mangaSlug, int? mangaId)
        {
            string? userId = _userManager.GetUserId(User);
            MangaDetailsViewModel viewModel = await _mangaService.GetMangaDetailsByIdAsync(mangaId, userId);
            if (!string.Equals(mangaSlug, viewModel.Manga.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction(nameof(Details), new { mangaSlug = viewModel.Manga.Slug, mangaId = viewModel.Manga.Id });
            }
            return View(viewModel);
        }
    }
}
