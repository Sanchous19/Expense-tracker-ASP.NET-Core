using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Tracker.Models;
using Tracker.ViewModels;

namespace Tracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ApplicationContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IStringLocalizer<AccountController> localizer, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _context = context;
        }

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Login };
                var result = await _userManager.CreateAsync(user, model.Password);
                _context.SaveChanges();

                if (result.Succeeded)
                {
                    ChatList commonChats = new ChatList();
                    _context.ChatLists.Add(commonChats);
                    commonChats.Owner = user;
                    user.CommonChats = commonChats;
                    _context.SaveChanges();

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Private", "Home", user);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult LogIn() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Private", "Home");
                }
                else
                {
                    ModelState.AddModelError("", _localizer["IncorrectLoginOrPassword"]);
                }
            }
            return View(model);
        }
        
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }
    }
}