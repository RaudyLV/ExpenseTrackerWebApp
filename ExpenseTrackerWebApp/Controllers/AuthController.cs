using ExpenseTrackerWebApp.Services;
using ExpenseTrackerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExpenseTrackerWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM user)
        {

            var (result, errorMessage) = await _authService.LoginAsync(user);
            if(result.Succeeded)
            {
               return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", errorMessage);

            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password) )
            {
                return View();
            }

            var result = await _authService.RegisterAsync(user);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }


			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _authService.LogOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
