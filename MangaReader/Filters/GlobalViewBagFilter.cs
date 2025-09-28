using MangaReader.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MangaReader.Filters
{
    public class GlobalViewBagFilter : IActionFilter
    {
        private readonly IAppConfigService _appConfigService;

        public GlobalViewBagFilter(IAppConfigService appConfigService)
        {
            _appConfigService = appConfigService;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            // nothing :)
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                controller.ViewBag.BasePath = _appConfigService.GetBasePath();
                controller.ViewBag.CoversFolder = _appConfigService.GetCoversFolderName();
                controller.ViewBag.ChaptersFolder = _appConfigService.GetChaptersFolderName();
                controller.ViewBag.AvatarsFolder = _appConfigService.GetAvatarsFolderName();
            }
        }
    }
}
