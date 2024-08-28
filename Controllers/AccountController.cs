using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.ViewModels;


namespace NetflixSharp.Controllers

{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "ImageController");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "ImageController");
                }
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new EditProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return RedirectToAction("Login");

                user.UserName = model.Username;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "ImageConroller");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet("Delete")]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                await _signInManager.SignOutAsync();
                return RedirectToAction("SignUp");
            }

            return RedirectToAction("Login");
        }
    }
}