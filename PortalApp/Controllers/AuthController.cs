using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PortalApp.Models;



namespace PortalApp.Controllers
{
    public class AuthController : Controller
    {

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if(claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VMLogin loginModel)
        {
            if (loginModel.Email == "mamang@gmail.com" && loginModel.Password == "123")
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginModel.Email),
                    new Claim("Tena", "for testing login onlyu")
                };

                ClaimsIdentity claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = loginModel.KeepLoggedIn,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), properties);
                
                return RedirectToAction("Index", "Home");

            }

            ViewData["ValidateMessage"] = "Failed to Login, Wrong username/password!";
            return View();
        }

    }
}
