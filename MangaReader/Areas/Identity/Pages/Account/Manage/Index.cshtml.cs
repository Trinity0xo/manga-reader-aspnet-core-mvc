// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using MangaReader.Models;
using MangaReader.Services;
using MangaReader.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MangaReader.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IFileService _fileService;
        private readonly IAppConfigService _appConfigService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager, IFileService fileService, IAppConfigService appConfigService, ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
            _appConfigService = appConfigService;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        public string OldAvatar { get; set; }

        public string BasePath { get; set; }

        public string AvatarsFolder { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel : IValidatableObject
        {
            [DisplayName("Ảnh đại diện")]
            public IFormFile? NewAvatar { get; set; }

            [Required(ErrorMessage = "Họ là bắt buộc")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Họ phải từ 2 đến 50 ký tự")]
            [DisplayName("Họ")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Tên là bắt buộc")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Tên phải từ 2 đến 50 ký tự")]
            [DisplayName("Tên")]
            public string FirstName { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (NewAvatar != null)
                {
                    if (!ImageUtils.IsValidImageType(NewAvatar.ContentType))
                    {
                        yield return new ValidationResult(
                            "File ảnh không hợp lệ",
                            [nameof(NewAvatar)]
                        );
                    }
                }
            }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;
            OldAvatar = user.Avatar;
            BasePath = _appConfigService.GetBasePath();
            AvatarsFolder = _appConfigService.GetAvatarsFolderName();

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (user.FirstName != Input.FirstName || user.LastName != Input.LastName)
            {
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
            }

            if (Input.NewAvatar != null)
            {
                string? newAvatar = await _fileService.UploadUserAvatar(Input.NewAvatar);
                if (string.IsNullOrEmpty(newAvatar))
                {
                    throw new InvalidOperationException("Không thể tải ảnh đại diện");
                }

                if (!string.IsNullOrEmpty(newAvatar))
                {
                    _fileService.DeleteUserAvatar(user.Avatar);
                }

                user.Avatar = newAvatar;
            }

            user.UpdatedAt = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Cập nhật thông tin hồ sơ thành công";
            return RedirectToPage();
        }
    }
}
