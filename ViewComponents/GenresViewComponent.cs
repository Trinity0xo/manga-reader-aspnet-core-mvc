using Microsoft.AspNetCore.Mvc;
using WEBTRUYEN.Repository;

namespace WEBTRUYEN
{
    public class GenresViewComponent : ViewComponent
    {
        private readonly IGenreRepository _genreRepository;

        public GenresViewComponent(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _genreRepository.GetAllNoPaginateAsync();
            return View(genres);
        }
    }
}