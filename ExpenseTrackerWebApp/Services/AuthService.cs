using ExpenseTrackerWebApp.Models;
using ExpenseTrackerWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackerWebApp.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<(SignInResult Result, string ErrorMessage)> LoginAsync(LoginVM loginVM)
        { 
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user == null)
            {
                return (SignInResult.Failed, "El email no está registrado.");
            }

            var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.IsPersistent, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return (result, "Email o contraseña incorrectos.");
            }

            return (result, string.Empty);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterVM userVM)
        {
            var findUser = await _userManager.FindByEmailAsync(userVM.Email);
            if (findUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email already exists." });
            }

            var user = new User { Email = userVM.Email, UserName = userVM.Email, FullName = userVM.FullName };
            var result = await _userManager.CreateAsync(user, userVM.Password);

            return result;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
