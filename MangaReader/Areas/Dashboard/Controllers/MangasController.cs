using MangaReader.Dto;
using MangaReader.Dto.Chapter;
using MangaReader.Dto.Manga;
using MangaReader.Models;
using MangaReader.Services;
using MangaReader.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class MangasController : Controller
    {
        private readonly IMangaService _mangaService;
        private readonly IGenreService _genreService;
        private readonly IChapterService _chapterService;

        public MangasController(IMangaService mangaService, IGenreService genreService, IChapterService chapterService)
        {
            _mangaService = mangaService;
            _genreService = genreService;
            _chapterService = chapterService;
        }

        // GET: Mangas
        public async Task<IActionResult> Index(FilterMangaDto filterMangaDto)
        {
            PageResponse<IEnumerable<Manga>> mangas = await _mangaService.GetAllMangasAsync(filterMangaDto);
            ViewBag.FilterMangaDto = filterMangaDto;
            return View(mangas);
        }

        // GET: Mangas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Manga manga = await _mangaService.GetMangaDetailsByIdAsync(id);
            return View(manga);
        }

        // GET: Mangas/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _genreService.GetAllGenresAsync();
            return View();
        }

        // POST: Mangas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMangaDto createMangaDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = await _genreService.GetAllGenresAsync();
                return View(createMangaDto);
            }

            bool existsTitle = await _mangaService.ExistsByTitleAsync(createMangaDto.Title);
            if (existsTitle) {
                ModelState.AddModelError(nameof(createMangaDto.Title), "Tên truyện đã tồn tại");
                ViewBag.Genres = await _genreService.GetAllGenresAsync();

                return View(createMangaDto);
            }

            await _mangaService.CreateNewMangaAsync(createMangaDto);
            TempData["SuccessMessage"] = "Tạo truyện mới thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: Mangas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UpdateMangaDto updateMangaDto = await _mangaService.GetMangaInfoForUpdateAsync(id);
            ViewBag.Genres = await _genreService.GetAllGenresAsync();

            return View(updateMangaDto);
        }

        // POST: Mangas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateMangaDto updateMangaDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = await _genreService.GetAllGenresAsync();
                UpdateMangaDto newUpdateMangaDto = await _mangaService.GetMangaInfoForUpdateAsync(id);
                updateMangaDto.OldCover = newUpdateMangaDto.OldCover;
                updateMangaDto.OldTitle = newUpdateMangaDto.OldTitle;

                return View(updateMangaDto);
            }

            Manga? existsManga = await _mangaService.GetMangaBySlugAsync(SlugUtils.GenerateSlug(updateMangaDto.NewTitle));
            if(existsManga != null && existsManga.Id != id)
            {
                ViewBag.Genres = await _genreService.GetAllGenresAsync();
                UpdateMangaDto newUpdateMangaDto = await _mangaService.GetMangaInfoForUpdateAsync(id);
                updateMangaDto.OldCover = newUpdateMangaDto.OldCover;
                updateMangaDto.OldTitle = newUpdateMangaDto.OldTitle;
                ModelState.AddModelError(nameof(updateMangaDto.NewTitle), "Tên truyện đã tồn tại");

                return View(updateMangaDto);

            }

            await _mangaService.UpdateMangaAsync(id, updateMangaDto);
            TempData["SuccessMessage"] = "Cập nhật truyện thành công";
            return RedirectToAction(nameof(Details), new { id = updateMangaDto.Id });
        }

        // GET: Mangas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Manga manga = await _mangaService.GetMangaInfoForDeleteAsync(id);

            return View(manga);
        }

        // POST: Mangas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mangaService.DeleteMangaAsync(id);
            TempData["SuccessMessage"] = "Xóa truyện thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}
