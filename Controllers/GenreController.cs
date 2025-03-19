using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{

    [Route("/admin/genre")]
    [Authorize(Roles = "admin")]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        // dependency injection
        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        //hiển thị trang có tất cả thể loại
        [HttpGet("")]
        public async Task<IActionResult> Index(string? searchValue, int pageNumber)
        {
            int pageSize = 15;
            var genres = await _genreRepository.GetAllAsync(pageSize, pageNumber, searchValue);

            var totalGenres = await _genreRepository.GetTotalCountAsync(searchValue);

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(totalGenres / (double)pageSize);
            ViewBag.searchValue = searchValue;

            return View(genres);
        }

        [HttpGet("create")]
        // hiển thị trang tạo mới một thể loại
        public IActionResult Create()
        {
            return View();
        }

        // gọi form có method post để thực hiện tạo mới một thể loại
        [HttpPost("create")]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreRepository.AddAsync(genre);

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        //hiển thị trang cập nhật 1 thể loại
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // gọi form có method post để thực hiện cập nhật một thể loại
        [HttpPost("edit"), ActionName("ConfirmEdit")]
        public async Task<IActionResult> Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreRepository.UpdateAsync(genre);

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        // hiển thị thông tin chi tiết của thể loại
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }
        // hiển thị trang xóa một thể loại
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // gọi form có method post để xác nhận xóa một thể loại
        [HttpPost("delete"), ActionName("ConfirmDelete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _genreRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
