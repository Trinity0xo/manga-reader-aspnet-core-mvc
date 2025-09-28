using MangaReader.Dto;
using MangaReader.Dto.Genre;
using MangaReader.Models;
using MangaReader.Services;
using MangaReader.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: Genres
        public async Task<IActionResult> Index(PageableDto pageableDto)
        {
            PageResponse<IEnumerable<Genre>> genres = await _genreService.GetAllGenresAsync(pageableDto);
            ViewBag.PageableDto = pageableDto;
            return View(genres);
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
             
            Genre genre = await _genreService.GetGenreDetailsByIdAsync(id);
         
            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreDto createGenreDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createGenreDto);
            }

            bool existsSlug = await _genreService.ExistsBySlugAsync(SlugUtils.GenerateSlug(createGenreDto.Name));
            if (existsSlug)
            {
                ModelState.AddModelError(nameof(createGenreDto.Name), "Tên thể loại đã tồn tại");
                return View(createGenreDto);
            }

            await _genreService.CreateNewGenreAsync(createGenreDto);
            TempData["SuccessMessage"] = "Tạo thể loại mới thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UpdateGenreDto updateGenreDto = await _genreService.GetInfoForUpdateGenreAsync(id);

            return View(updateGenreDto);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateGenreDto updateGenreDto)
        {
            if(id != updateGenreDto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                UpdateGenreDto newUpdateGenreDto = await _genreService.GetInfoForUpdateGenreAsync(updateGenreDto.Id);
                updateGenreDto.OldName = newUpdateGenreDto.OldName;
                return View(updateGenreDto);
            }

            Genre? existsGenre = await _genreService.GetGenreBySlugAsync(SlugUtils.GenerateSlug(updateGenreDto.NewName));
            if (existsGenre != null && existsGenre.Id != id)
            {
                UpdateGenreDto newUpdateGenreDto = await _genreService.GetInfoForUpdateGenreAsync(updateGenreDto.Id);
                updateGenreDto.OldName = newUpdateGenreDto.OldName;
                ModelState.AddModelError(nameof(updateGenreDto.NewName), "Tên thể loại đã tồn tại");
                return View(updateGenreDto);
            }

            await _genreService.UpdateGenreAsync(updateGenreDto);
            TempData["SuccessMessage"] = "Cập nhật thể loại thành công";
            return RedirectToAction(nameof(Details), new { id = updateGenreDto.Id });
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Genre genre = await _genreService.GetGenreDetailsByIdAsync(id);

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {  
            await _genreService.DeleteGenreAsync(id);
            TempData["SuccessMessage"] = "Xóa thể loại thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}
