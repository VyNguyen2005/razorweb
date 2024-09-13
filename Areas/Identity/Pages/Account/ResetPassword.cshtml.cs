
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using razor09_razorweb.models;

namespace razor09_razorweb.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập {0}")]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "Mật khẩu")]
            [Required(ErrorMessage = "Phải nhập {0}")]
            [StringLength(100, ErrorMessage = "{0} phải từ {2} đến {1} kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]

            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Xác thực mật khẩu")]
            [Compare("Password", ErrorMessage = "Xác thực lại mật khẩu sai.")]
            public string ConfirmPassword { get; set; }
            [Required]

            public string Code { get; set; }

        }
        // handler ResetPassword?code=Matoken
        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Phải cung cấp mã để đặt lại mật khẩu.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
