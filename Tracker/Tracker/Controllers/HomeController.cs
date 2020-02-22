using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;


namespace Tracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public HomeController(UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Private()
        {
            User user = await GetCurrentUser();
            _context.Entry(user).Collection("Records").Load();

            var records = Record.GroupByMonth(user.Records);
            return View(records);
        }

        [HttpGet]
        public IActionResult Family()
        {
            User user = _context.Users.First(u => u.UserName == HttpContext.User.Identity.Name);

            if (user.FamilyChatId != null)
            {
                return RedirectToAction("Index", "Chat", new { id = user.FamilyChatId });
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Common()
        {
            User user = await GetCurrentUser();
            _context.Entry(user).Reference("CommonChats").Load();
            //_context.Entry(user.CommonChats).Collection(chl => chl.Chats).Load();
            ChatList chatList = new ChatList();
            for (int i = 0; i < 10; i++)
            {
                Chat chat = new Chat() { Name = "Chat" + i.ToString() };
                chat.Members.Add(user);
                chat.Messages.Add(new Message() { Text = "Message" + i.ToString(), UserFrom = user });
                chatList.Chats.Add(chat);
            }

            return View(chatList);
        }

        [HttpPost]
        public IActionResult RecordTable(string recordIds)
        {
            List<string> ids = recordIds.Split('#').ToList();
            List<Record> records = new List<Record>();
            foreach (string id in ids)
            {
                records.Add(_context.Records.Find(id));
            }
            List<List<Record>> result = Record.GroupByDay(records);
            return PartialView("_RecordTable", result);
        }

        private async Task<User> GetCurrentUser() => await _userManager.GetUserAsync(HttpContext.User);
    }
}
