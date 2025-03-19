using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    [Route("/admin/comic")]
    [Authorize(Roles = "admin")]
    public class ComicController : Controller
    {
        private readonly IComicRepository _comicRepository;
        private readonly IGenreRepository _genreRepositroy;

        public ComicController(IComicRepository comicRepositroy, IGenreRepository genreRepositroy)
        {
            _comicRepository = comicRepositroy;
            _genreRepositroy = genreRepositroy;
        }

        private async Task<string> SavePageImage(IFormFile image)
        {
            var directoryPath = Path.Combine("wwwroot", "images", "comicCovers");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var savePath = Path.Combine(directoryPath, image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/comicCovers/" + image.FileName;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? searchValue, int pageNumber)
        {
            int pageSize = 15;
            var comics = await _comicRepository.GetAllAsync(pageSize, pageNumber, searchValue);

            var totalComics = await _comicRepository.GetTotalCountAsync(searchValue);

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(totalComics / (double)pageSize);
            ViewBag.searchValue = searchValue;

            return View(comics);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var genres = await _genreRepositroy.GetAllNoPaginateAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name");

            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Comic comic, List<int> Genres, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    comic.CoverUrl = await SavePageImage(image);
                }

                comic.Genres = await _genreRepositroy.GetByIdAsync(Genres);
                await _comicRepository.AddAsync(comic);

                return RedirectToAction(nameof(Index));
            }

            var genres = await _genreRepositroy.GetAllNoPaginateAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name");

            return View(comic);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            var selectedGenres = comic.Genres.Select(g => g.Id).ToList();

            var genres = await _genreRepositroy.GetAllNoPaginateAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name", selectedGenres);

            return View(comic);
        }

        [HttpPost("edit"), ActionName("ConfirmEdit")]
        public async Task<IActionResult> Edit(Comic comic, List<int> Genres, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                var comicDb = await _comicRepository.GetByIdAsync(comic.Id);
                if (comicDb == null)
                {
                    return NotFound();
                }

                if (image != null)
                {
                    comicDb.CoverUrl = await SavePageImage(image);
                }

                comicDb.Name = comic.Name;
                comicDb.Description = comic.Description;
                comicDb.Author = comic.Author;
                comicDb.PublicAt = comic.PublicAt;
                comicDb.Status = comic.Status;
                comicDb.Genres = await _genreRepositroy.GetByIdAsync(Genres);

                await _comicRepository.UpdateAsync(comicDb);

                return RedirectToAction(nameof(Index));
            }

            var selectedGenres = comic.Genres.Select(g => g.Id).ToList();

            if (selectedGenres == null)
            {
                selectedGenres = new List<int>();
            }

            var genres = await _genreRepositroy.GetAllNoPaginateAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name", selectedGenres);

            return View(comic);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comic = await _comicRepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        [HttpPost("delete"), ActionName("ConfirmDelete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _comicRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
