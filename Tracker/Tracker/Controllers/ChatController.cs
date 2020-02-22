using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Tracker.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public ChatController(UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id, bool isOpenModal = false, string modalText = "")
        {
            User user = await GetCurrentUser();
            ViewBag.UserId = user.Id;
            Chat chat = _context.Chats.Find(id);
            _context.Entry(chat).Collection("Messages").Load();
            chat.Messages.Sort(new Message.Comparer());
            foreach (var message in chat.Messages)
            {
                _context.Entry(message).Reference("UserFrom").Load();
            }

            if (isOpenModal)
            {
                ViewBag.IsOpenModal = true;
                ViewBag.ModalText = modalText;
            }
            else
            {
                ViewBag.IsOpenModal = false;
            }

            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name = "", bool isFamily = false)
        {
            User user = await GetCurrentUser();
            Chat chat = new Chat() { Name = name };
            chat.Members.Add(user);
            //for (int i = 0; i < 100; i++)
            //{
            //    var message = new Message() { UserFrom = user, Text = "cghjbklm;khuyfytgvyiughiuhiufhuthigerhkgh" + i.ToString() };
            //    _context.Messages.Add(message);
            //    chat.Messages.Add(message);
            //}
            _context.Chats.Add(chat);
            _context.SaveChanges();
            if (isFamily)
            {
                user.FamilyChat = chat;
            }
            else
            {
                _context.Entry(user).Reference("CommonChats").Load();
                //_context.Entry(user.CommonChats).Collection("Chats").Load();
                user.CommonChats.Chats.Add(chat);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Chat", new { chat.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Exit(string id)
        {
            User user = await GetCurrentUser();
            Chat chat = _context.Chats.Find(id);

            bool isFamily = false;
            if (user.FamilyChatId == chat.Id)
            {
                user.FamilyChat = null;
                isFamily = true;
            }
            else
            {
                _context.Entry(user).Collection("CommonChats").Load();
                ChatList commonChats = user.CommonChats;
                _context.Entry(commonChats).Collection("Chats").Load();
                commonChats.Chats.Remove(chat);
            }
            
            _context.Entry(chat).Collection("Members").Load();
            chat.Members.Remove(user);
            if (chat.Members.Count == 0)
            {
                _context.Entry(chat).Collection("Messages").Load();
                foreach (var message in chat.Messages)
                {
                    _context.Messages.Remove(message);
                }
                _context.Chats.Remove(chat);
            }

            _context.SaveChanges();

            if (isFamily)
            {
                return RedirectToAction("Family", "Home");
            }
            else
            {
                return RedirectToAction("Common", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Invite(string id, string userName)
        {
            bool isOpenModal = true, isRight = false;
            string modalText = "";

            User user = await GetCurrentUser();
            bool isFamily = user.FamilyChatId == id ? true : false;
            Chat chat = _context.Chats.Find(id);
            _context.Entry(chat).Collection("Members").Load();
            User invitedUser = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (invitedUser == null)
            {
                modalText = "Пользователя с таким никнеймом не существует";
            }
            else if (isFamily && invitedUser.FamilyChatId != null)
            {
                modalText = "Пользователя с таким никнеймом не существует";
            }
            else if (chat.Members.Contains(invitedUser))
            {
                modalText = "Пользователя с таким никнеймом не существует";
            }
            else
            {
                isRight = true;
                modalText = "Пользователь добавлен";
            }

            if (isRight)
            {
                chat.Members.Add(invitedUser);
                if (isFamily)
                {
                    invitedUser.FamilyChat = chat;
                }
                else
                {
                    _context.Entry(invitedUser).Collection("CommonChats").Load();
                    ChatList commonChats = invitedUser.CommonChats;
                    _context.Entry(commonChats).Collection("Chats").Load();
                    commonChats.Chats.Remove(chat);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Chat", new { id, isOpenModal, modalText });
        }

        private async Task<User> GetCurrentUser() => await _userManager.GetUserAsync(HttpContext.User);
    }
}