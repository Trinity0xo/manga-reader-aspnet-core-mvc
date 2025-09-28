using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaReader.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/Error/{statusCode}")]
        [AllowAnonymous]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var originalPath = statusCodeFeature?.OriginalPath;

            switch (statusCode)
            {
                case 400:
                    Response.StatusCode = statusCode;
                    ViewBag.ErrorType = "Bad Request";
                    ViewBag.ErrorStatusCode = statusCode;
                    ViewBag.ErrorMessage = "Yêu cầu của bạn không hợp lệ";
                    _logger.LogWarning("400 error occurred on path: {Path}", originalPath);
                    break;

                case 404:
                    Response.StatusCode = statusCode;
                    ViewBag.ErrorType = "Not Found";
                    ViewBag.ErrorStatusCode = statusCode;
                    ViewBag.ErrorMessage = "Trang bạn truy cập không tồn tại";
                    _logger.LogWarning("404 error occurred on path: {Path}", originalPath);
                    break;
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        [Route("/Error")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleException()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (exceptionDetails?.Error is UnauthorizedAccessException)
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (exceptionDetails?.Error is KeyNotFoundException)
            {
                Response.StatusCode = 404;
                ViewBag.ErrorStatusCode = Response.StatusCode;
                ViewBag.ErrorType = "Not Found";
                ViewBag.ErrorMessage = exceptionDetails.Error.Message;
            }

            else if (exceptionDetails?.Error is InvalidOperationException)
            {
                Response.StatusCode = 409;
                ViewBag.ErrorStatusCode = Response.StatusCode;
                ViewBag.ErrorType = "Bad Request";
                ViewBag.ErrorMessage = exceptionDetails.Error.Message;
            }

            else
            {
                Response.StatusCode = 500;
                ViewBag.ErrorStatusCode = Response.StatusCode;
                ViewBag.ErrorType = "Internal Error";
                ViewBag.ErrorMessage = "Đã xảy ra lỗi vui lòng thử lại sau";
                _logger.LogError(exceptionDetails?.Error, "An unhandled exception occurred on path: {Path}", exceptionDetails?.Path);
            }

            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
