using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tracker.Models;


namespace Tracker.Utils
{
    public class Initializer
    {
        private const string _adminName = "sasha1";
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;

        public Initializer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task Initialize()
        {
            string admin = UserRole.Admin.ToString(), moderator = UserRole.Moderator.ToString(), blocked_user = UserRole.BlockedUser.ToString();
            
            if (await _roleManager.FindByNameAsync(admin) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(admin));
            }
            if (await _roleManager.FindByNameAsync(moderator) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(moderator));
            }
            if (await _roleManager.FindByNameAsync(blocked_user) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(blocked_user));
            }

            await _userManager.CreateAsync(new User { UserName = _adminName }, _adminName);
            User user = await _userManager.FindByNameAsync(_adminName);
            await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());

            List<Record> records = new List<Record>()
            {
                new Record("Батон", 1.2, new DateTime(2019, 11, 8), "Мама попросила"),
                new Record("Молоко", 0.9, new DateTime(2019, 11, 8), "Мама попросила"),
                new Record("Сыр", 3, new DateTime(2019, 11, 8), "Мама попросила"),
                new Record("Сметана", 2, new DateTime(2019, 11, 8), "Мама попросила"),
                new Record("Мороженное", 1.1, new DateTime(2019, 11, 8)),
                new Record("Кроссовки", 200, new DateTime(2019, 11, 5)),
                new Record("Шорты", 40, new DateTime(2019, 11, 5)),
                new Record("Футболка", 30, new DateTime(2019, 11, 5)),
                new Record("Батон", 1.2, new DateTime(2019, 11, 3), "Мама попросила"),
                new Record("Молоко", 0.9, new DateTime(2019, 11, 3), "Мама попросила"),
                new Record("Сыр", 3, new DateTime(2019, 11, 3), "Мама попросила"),
                new Record("Сметана", 2, new DateTime(2019, 11, 3), "Мама попросила"),
                new Record("Чипсы", 1.2, new DateTime(2019, 10, 29), "Футбол"),
                new Record("Пиво", 0.9, new DateTime(2019, 10, 29), "Футбол"),
                new Record("Орешки", 3, new DateTime(2019, 10, 29), "Футбол")
            };

            foreach (var record in records)
            {
                _context.Records.Add(record);
                user.Records.Add(record);
            }
            _context.SaveChanges();
        }
    }
}
