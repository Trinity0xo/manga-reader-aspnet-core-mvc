using MangaReader.Dto;
using MangaReader.Dto.Chapter;
using MangaReader.Models;
using MangaReader.Services;
using MangaReader.Utils;
using MangaReader.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ChaptersController : Controller
    {
        private readonly IChapterService _chapterService;

        public ChaptersController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        // GET: Chapters
        public async Task<IActionResult> Index(PageableDto pageableDto)
        {
            PageResponse<IEnumerable<Chapter>> chapters = await _chapterService.GetNewestChaptersAsync(pageableDto);
            ViewBag.PageableDto = pageableDto;
            return View(chapters);
        }

        // GET: Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Chapter chapter = await _chapterService.GetChapterDetailsByIdAsync(id);
            return View(chapter);
        }

        // GET: Chapters/MangaChapters/5
        public async Task<IActionResult> MangaChapters(int? mangaId, PageableDto pageableDto)
        {
            if (mangaId == null)
            {
                return NotFound();
            }

            MangaChaptersViewModel result = await _chapterService.GetMangaChaptersAsync(mangaId, pageableDto);
            ViewBag.PageableDto = pageableDto;
            return View(result);
        }

        // GET: Chapters/Create
        public async Task<IActionResult> Create(int? mangaId)
        {
            if (mangaId == null)
            {
                return NotFound();
            }

            CreateChapterDto createChapterDto = await _chapterService.GetInfoForCreateChapterAsync(mangaId);
            return View(createChapterDto);
        }

        // POST: Chapters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int mangaId, CreateChapterDto createChapterDto)
        {
            if (!ModelState.IsValid)
            {
                CreateChapterDto newCreateChapterDto = await _chapterService.GetInfoForCreateChapterAsync(mangaId);
                createChapterDto.MangaId = newCreateChapterDto.MangaId;
                createChapterDto.MangaTitle = newCreateChapterDto.MangaTitle;
                createChapterDto.MangaSlug = newCreateChapterDto.MangaSlug;

                return View(createChapterDto);
            }

            bool existsSlug = await _chapterService.ExistsByMangaIdAndChapterSlugAsync(createChapterDto.MangaId, SlugUtils.GenerateSlug(createChapterDto.Title));
            if (existsSlug)
            {
                CreateChapterDto newCreateChapterDto = await _chapterService.GetInfoForCreateChapterAsync(mangaId);
                createChapterDto.MangaId = newCreateChapterDto.MangaId;
                createChapterDto.MangaTitle = newCreateChapterDto.MangaTitle;
                createChapterDto.MangaSlug = newCreateChapterDto.MangaSlug;
                ModelState.AddModelError(nameof(createChapterDto.Title), "Tên chương đã tồn tại");

                return View(createChapterDto);
            }

            await _chapterService.CreateNewChapterAsync(mangaId, createChapterDto);
            TempData["SuccessMessage"] = "Tạo chương mới thành công";
            return RedirectToAction(nameof(MangaChapters), new { createChapterDto.MangaId });
        }

        // GET: Chapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UpdateChapterDto updateChapterDto = await _chapterService.GetInfoForUpdateChapterAsync(id);
            return View(updateChapterDto);
        }

        // POST: Chapters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateChapterDto updateChapterDto)
        {
            if (!ModelState.IsValid)
            {
                UpdateChapterDto newUpdateChapterDto = await _chapterService.GetInfoForUpdateChapterAsync(id);
                updateChapterDto.OldPages = newUpdateChapterDto.OldPages;
                updateChapterDto.OldTitle = newUpdateChapterDto.OldTitle;
                updateChapterDto.MangaId = newUpdateChapterDto.MangaId;
                updateChapterDto.MangaTitle = newUpdateChapterDto.MangaTitle;
                updateChapterDto.MangaSlug = newUpdateChapterDto.MangaSlug;

                return View(updateChapterDto);
            }

            Chapter? existsChapter = await _chapterService.GetChapterByMangaIdAndChapterSlugAsync(updateChapterDto.MangaId, SlugUtils.GenerateSlug(updateChapterDto.NewTitle));
            if (existsChapter != null && existsChapter.Id != id)
            {
                UpdateChapterDto newUpdateChapterDto = await _chapterService.GetInfoForUpdateChapterAsync(id);
                updateChapterDto.OldPages = newUpdateChapterDto.OldPages;
                updateChapterDto.OldTitle = newUpdateChapterDto.OldTitle;
                updateChapterDto.MangaId = newUpdateChapterDto.MangaId;
                updateChapterDto.MangaTitle = newUpdateChapterDto.MangaTitle;
                updateChapterDto.MangaSlug = newUpdateChapterDto.MangaSlug;
                ModelState.AddModelError(nameof(updateChapterDto.NewTitle), "Tên chương đã tồn tại");

                return View(updateChapterDto);
            }

            await _chapterService.UpdateChapterAsync(id, updateChapterDto);
            TempData["SuccessMessage"] = "Cập nhật chương thành công";
            return RedirectToAction(nameof(MangaChapters), new { updateChapterDto.MangaId });
        }

        // GET: Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Chapter chapter = await _chapterService.GetInfoForDeleteChapterAsync(id);
            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int mangaId)
        {
            await _chapterService.DeleteChapterAsync(id);
            TempData["SuccessMessage"] = "Xóa chương thành công";
            return RedirectToAction(nameof(MangaChapters), new { mangaId });
        }
    }
}
