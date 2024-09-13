


// #nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using razor09_razorweb.models;

namespace razor09_razorweb.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        public string RetunUrl { get; set; }

        public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // var user = new AppUser{UserName = Input.UserName, Email = Input.Email}
            // Đăng kí var result = await _signInManager.CreateAsync(user, Input.Password) (Đăng kí với user và password)
            // Đăng nhập var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, LockoutOnFailure: false)
            // Đăng xuất await _signInManager.SignOutAsync();
            // if(returnUrl != null){
            //    return LocalRedirect(returnUrl);
            //}
            // else{
            //    RedirectToPage();
            //}
            await _signInManager.SignOutAsync();

            _logger.LogInformation("Đã đăng xuất tài khoản");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                returnUrl = Url.Content("~/");
                return LocalRedirect(returnUrl);
            }
        }
    }
}
