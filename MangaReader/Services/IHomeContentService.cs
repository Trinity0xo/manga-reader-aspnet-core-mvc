using MangaReader.ViewModels;

namespace MangaReader.Services
{
    public interface IHomeContentService
    {
        Task<HomeContentViewModel> GetHomeContentAsync();
        Task<DashboardContentViewModel> GetDashboardContentAsync();
    }
}
