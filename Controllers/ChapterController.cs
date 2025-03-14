using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBTRUYEN.Models;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN.Controllers
{
    [Route("/admin/chapter")]
    [Authorize(Roles = "admin")]
    public class ChapterController : Controller
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IComicRepository _comicRepository;

        public ChapterController(IChapterRepository chapterRepository, IComicRepository comicRepository)
        {
            _chapterRepository = chapterRepository;
            _comicRepository = comicRepository;
        }

        [HttpGet("create/{comicId}")]
        public async Task<IActionResult> Create(int comicId)
        {
            var comic = await _comicRepository.GetByIdAsync(comicId);

            if (comic == null) {
                return NotFound();
            }

            var chapter = new Chapter { ComicId = comicId };

            return View(chapter);
        }

        [HttpPost("create"),ActionName("ConfirmCreate")]
        public async Task<IActionResult> Create(Chapter chapter)
        {

            if (ModelState.IsValid)
            {
                await _chapterRepository.AddAsync(chapter);

                return RedirectToAction("Details", "Comic", new { id = chapter.ComicId });
            }

            return View(chapter);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var chapter = await _chapterRepository.GetByIdAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var chapter = await _chapterRepository.GetByIdAsync(id);

            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        [HttpPost("edit"), ActionName("ConfirmEdit")]
        public async Task<IActionResult> Edit(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                var chapterDb = await _chapterRepository.GetByIdAsync(chapter.Id);
                if (chapterDb == null)
                {
                    return NotFound();
                }

                chapterDb.Name = chapter.Name;

                await _chapterRepository.UpdateAsync(chapterDb);

                return RedirectToAction("Details", "Comic", new { id = chapterDb.ComicId });
            }

            return View(chapter);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comic = await _chapterRepository.GetByIdAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        [HttpPost("delete"), ActionName("ConfirmDelete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var chapter = await _chapterRepository.GetByIdAsync(id);

            if (chapter == null)
            {
                return NotFound();
            }

            var comicId = chapter.ComicId;

            await _chapterRepository.DeleteAsync(id);


            return RedirectToAction("Details", "Comic", new { id = comicId });
        }
    }
}
