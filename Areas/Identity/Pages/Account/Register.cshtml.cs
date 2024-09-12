
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using razor09_razorweb.models;

namespace razor09_razorweb.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IUserStore<AppUser> userStore, ILogger<RegisterModel> logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _logger = logger;
            _emailSender = emailSender;
        }

        // Liên kết dữ liệu đến các property của Model
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        // Chứa thông tin cơ chế xác thực
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập {0}")]
            [Display(Name = "Tên tài khoản")]
            [StringLength(100, ErrorMessage = "{0} phải từ {2} đến {1} kí tự", MinimumLength = 6)]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Phải nhập {0}")]
            [EmailAddress(ErrorMessage = "Sai định dạng {0}")]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Phải nhập {0}")]
            [StringLength(100, ErrorMessage = "{0} phải từ {2} đến {1} kí tự", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Xác thực lại mật khẩu")]
            [Compare("Password", ErrorMessage = "Xác thực lại mật khẩu sai")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // returnUrl == null -> ReturnUrl sẽ được gán giá trị là URL gốc của ứng dụng (root path)
            ReturnUrl = returnUrl != null ? returnUrl : Url.Content("~/");
            // returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AppUser{UserName = Input.UserName, Email = Input.Email};
                // var user = CreateUser();

                // await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                // await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Tạo tài khoản mới thành công");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận tài khoản",
                        $"Bạn đã tạo tài khoản mới. Nhấn vào <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>để kích hoạt tài khoản</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        // private AppUser CreateUser()
        // {
        //     try
        //     {
        //         return Activator.CreateInstance<AppUser>();
        //     }
        //     catch
        //     {
        //         throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
        //             $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
        //             $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        //     }
        // }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }
    }
}
