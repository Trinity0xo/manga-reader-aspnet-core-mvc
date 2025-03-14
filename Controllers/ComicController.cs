using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    [Route("/admin/comic")]
    [Authorize(Roles ="admin")]
    public class ComicController : Controller
    {
        private readonly IComicRepository _comicRepositroy;
        private readonly IGenreRepository _genreRepositroy;

        public ComicController(IComicRepository comicRepositroy, IGenreRepository genreRepositroy)
        {
            _comicRepositroy = comicRepositroy;
            _genreRepositroy = genreRepositroy;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var comics = await _comicRepositroy.GetAllAsync();

            return View(comics);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var genres = await _genreRepositroy.GetAllAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name");

            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Comic comic, List<int> Genres)
        {
            if (ModelState.IsValid)
            {
                comic.Genres = await _genreRepositroy.GetByIdAsync(Genres);
                await _comicRepositroy.AddAsync(comic);

                return RedirectToAction(nameof(Index));
            }

            var genres = await _genreRepositroy.GetAllAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name");

            return View(comic);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var comic = await _comicRepositroy.GetByIdAsync(id);
            if(comic == null)
            {
                return NotFound();
            }

            var selectedGenres = comic.Genres.Select(g => g.Id).ToList();

            var genres = await _genreRepositroy.GetAllAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name", selectedGenres);

            return View(comic);
        }

        [HttpPost("edit"), ActionName("ConfirmEdit")]
        public async Task<IActionResult> Edit(Comic comic, List<int> Genres)
        {
            if (ModelState.IsValid)
            {
                var comicDb = await _comicRepositroy.GetByIdAsync(comic.Id);
                if (comicDb == null) { 
                    return NotFound();
                }

                comicDb.Name = comic.Name;
                comicDb.Description = comic.Description;
                comicDb.Author = comic.Author;
                comicDb.PublicAt = comic.PublicAt;
                comicDb.Status = comic.Status;
                comicDb.Genres = await _genreRepositroy.GetByIdAsync(Genres);

                await _comicRepositroy.UpdateAsync(comicDb);

                return RedirectToAction(nameof(Index));
            }

            var selectedGenres = comic.Genres.Select(g => g.Id).ToList();

            var genres = await _genreRepositroy.GetAllAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name", selectedGenres);

            return View(comic);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var comic = await _comicRepositroy.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comic = await _comicRepositroy.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        [HttpPost("delete"), ActionName("ConfirmDelete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _comicRepositroy.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
