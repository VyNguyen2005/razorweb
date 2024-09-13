

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor09_razorweb.models;

namespace razor09_razorweb.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        public readonly SignInManager<AppUser> SignInManager;
        public readonly ILogger<LoginModel> _logger;
        public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger)
        {
            SignInManager = signInManager;
            _logger = logger;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập {0}")]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "Mật khẩu")]
            [Required(ErrorMessage = "Phải nhập {0}")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl != null ? returnUrl : Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl != null ? returnUrl : Url.Content("~/");

            ExternalLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if(result.Succeeded){
                    _logger.LogInformation("Đã đăng nhập thành công");
                    return RedirectToPage(returnUrl);
                }
                // Yêu cầu xác thực 2 yếu tố
                if(result.RequiresTwoFactor){
                    return RedirectToPage("./LoginWith2fa", new {ReturnUrl = returnUrl, RememberMe = Input.RememberMe});
                }
                if(result.IsLockedOut){
                    _logger.LogWarning("Tài khoản đã bị khóa");
                    return RedirectToPage("./Lockout");
                }
                else{
                    ModelState.AddModelError(string.Empty, "Đã cố gắng đăng nhập. Nhưng không hợp lệ");
                    return Page();
                }
            }
            return Page();
        }
    }
}


// #nullable disable

// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.UI.Services;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.Extensions.Logging;
// using razor09_razorweb.models;

// namespace razor09_razorweb.Areas.Identity.Pages.Account
// {
//     public class LoginModel : PageModel
//     {
//         private readonly SignInManager<AppUser> _signInManager;
//         private readonly ILogger<LoginModel> _logger;

//         public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger)
//         {
//             _signInManager = signInManager;
//             _logger = logger;
//         }

//         [BindProperty]
//         public InputModel Input { get; set; }

//         public IList<AuthenticationScheme> ExternalLogins { get; set; }

//         public string ReturnUrl { get; set; }

//         [TempData]
//         public string ErrorMessage { get; set; }

//         public class InputModel
//         {
//             [Required(ErrorMessage = "Phải nhập {0}")]
//             [EmailAddress]
//             public string Email { get; set; }

//             [Required(ErrorMessage = "Phải nhập {0}")]
//             [DataType(DataType.Password)]
//             [Display(Name = "Mật khẩu")]
//             public string Password { get; set; }

//             [Display(Name = "Remember me?")]
//             public bool RememberMe { get; set; }
//         }

//         public async Task OnGetAsync(string returnUrl = null)
//         {
//             if (!string.IsNullOrEmpty(ErrorMessage))
//             {
//                 ModelState.AddModelError(string.Empty, ErrorMessage);
//             }

//             ReturnUrl = returnUrl != null ? returnUrl : Url.Content("~/");

//             await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

//             ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

//             ReturnUrl = returnUrl;
//         }

//         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
//         {
//             // returnUrl ??= Url.Content("~/");
//             ReturnUrl = returnUrl != null ? returnUrl : Url.Content("~/");

//             ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

//             if (ModelState.IsValid)
//             {

//                 var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
//                 if (result.Succeeded)
//                 {
//                     _logger.LogInformation("Đã đăng nhập tài khoản.");
//                     return LocalRedirect(returnUrl);
//                 }
//                 if (result.RequiresTwoFactor)
//                 {
//                     return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
//                 }
//                 if (result.IsLockedOut)
//                 {
//                     _logger.LogWarning("Tài khoản người dùng bị khóa.");
//                     return RedirectToPage("./Lockout");
//                 }
//                 else
//                 {
//                     ModelState.AddModelError(string.Empty, "Đã cố gắng đăng nhập. Nhưng không hợp lệ");
//                     return Page();
//                 }
//             }

//             return Page();
//         }
//     }
// }
