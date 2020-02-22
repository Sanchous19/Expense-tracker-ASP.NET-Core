using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Tracker.Models;
using Tracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace Tracker.Controllers
{
    [Authorize]
    public class InternalRecordController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ApplicationContext _context;

        public InternalRecordController(UserManager<User> userManager, IStringLocalizer<AccountController> localizer, ApplicationContext context)
        {
            _userManager = userManager;
            _localizer = localizer;
            _context = context;
        }

        [HttpGet]
        public IActionResult Add(string returnUrl = null) => View(new RecordViewModel { ReturnUrl = returnUrl });
        
        [HttpPost]
        public async Task<IActionResult> Add(RecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await GetCurrentUser();
                _context.Entry(user).Collection("Records").Load();
                Record record = new Record(user, model.Thing, model.Cost, model.Date, model.Description);
                _context.Records.Add(record);
                user.Records.Add(record);
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Private", "Home");
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            Record record = _context.Records.Find(id);
            _context.Entry(record).Reference("Owner").Load();
            User user = record.Owner;
            _context.Entry(user).Collection("Records").Load();
            user.Records.Remove(record);
            _context.Records.Remove(record);
            _context.SaveChanges();
            return RedirectToAction("Private", "Home");
        }

        private async Task<User> GetCurrentUser() => await _userManager.GetUserAsync(HttpContext.User);
    }
}