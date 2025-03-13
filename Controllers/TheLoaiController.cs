using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{

    [Route("/admin/theloai")]
    [Authorize(Roles = "admin")]
    public class TheLoaiController : Controller
    {
        private readonly ITheLoaiRepository _theloaiRepository;

        // dependency injection
        public TheLoaiController(ITheLoaiRepository theloaiRepository)
        {
            _theloaiRepository = theloaiRepository;
        }

        //hiển thị trang có tất cả thể loại
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var theLoais = await _theloaiRepository.GetAllAsync();
            return View(theLoais);
        }

        [HttpGet("create")]
        // hiển thị trang tạo mới một thể loại
        public IActionResult Create()
        {
            return View();
        }

        // gọi form có method post để thực hiện tạo mới một thể loại
        [HttpPost("create")]
        public async Task<IActionResult> Create(TheLoai theLoai)
        {
            if (ModelState.IsValid) {
                await _theloaiRepository.AddAsync(theLoai);

                return RedirectToAction(nameof(Index));
            }

            return View(theLoai);
        }

        //hiển thị trang cập nhật 1 thể loại
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var theLoai = await _theloaiRepository.GetByIdAsync(id);
            if (theLoai == null) {
                return NotFound();
            }

            return View(theLoai);
        }

        // gọi form có method post để thực hiện cập nhật một thể loại
        [HttpPost("edit"), ActionName("ConfirmEdit")]
        public async Task<IActionResult> Edit(TheLoai theLoai)
        {
            if (ModelState.IsValid)
            {
                await _theloaiRepository.UpdateAsync(theLoai);

                return RedirectToAction(nameof(Index));
            }

            return View(theLoai);
        }

        // hiển thị thông tin chi tiết của thể loại
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var theLoai = await _theloaiRepository.GetByIdAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }

            return View(theLoai);
        }
        // hiển thị trang xóa một thể loại
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var theLoai = await _theloaiRepository.GetByIdAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }

            return View(theLoai);
        }

        // gọi form có method post để xác nhận xóa một thể loại
        [HttpPost("delete"), ActionName("ConfirmDelete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _theloaiRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
