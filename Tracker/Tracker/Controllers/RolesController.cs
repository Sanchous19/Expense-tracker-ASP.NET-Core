using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tracker.Models;
using Tracker.ViewModels;


namespace Tracker.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<User> _userManager;

        public RolesController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Admin() => View(await GetAdminAndModeratorRoles());

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> Moderator() => View(await GetBlockedUserRoles());

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AdminRole(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            string role = UserRole.Admin.ToString();
            await SetOrRemoveRole(user, role);
            return View("Admin", await GetAdminAndModeratorRoles());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ModeratorRole(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            string role = UserRole.Moderator.ToString();
            await SetOrRemoveRole(user, role);
            return View("Admin", await GetAdminAndModeratorRoles());
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<IActionResult> BlockedUserRole(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            string role = UserRole.BlockedUser.ToString();
            await SetOrRemoveRole(user, role);
            return View("Moderator", await GetBlockedUserRoles());
        }

        private async Task SetOrRemoveRole(User user, string role)
        {
            bool isHasRole = await _userManager.IsInRoleAsync(user, role);
            if (isHasRole)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        private async Task<List<UserRolesViewModel>> GetAdminAndModeratorRoles()
        {
            List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, UserRole.Admin.ToString());
                bool isModerator = await _userManager.IsInRoleAsync(user, UserRole.Moderator.ToString());
                userRoles.Add(new UserRolesViewModel(user.Id, user.UserName, isAdmin, isModerator));
            }
            return userRoles;
        }

        private async Task<List<UserRolesViewModel>> GetBlockedUserRoles()
        {
            List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                bool isBlockedUser = await _userManager.IsInRoleAsync(user, UserRole.BlockedUser.ToString());
                userRoles.Add(new UserRolesViewModel(user.Id, user.UserName, isBlockedUser));
            }
            return userRoles;
        }
    }
}