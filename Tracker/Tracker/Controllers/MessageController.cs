using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace Tracker.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private const int pageSize = 10;
        private readonly ApplicationContext _context;

        public MessageController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index(string chatId, int page = 0)
        {
            Chat chat = _context.Chats.Find(chatId);
            _context.Entry(chat).Collection("Messages").Load();
            chat.Messages.Sort(new Message.Comparer());
            var itemsToSkip = pageSize * page;
            List<Message> messages = chat.Messages.Skip(itemsToSkip).Take(pageSize).ToList();
            return PartialView("MessageList", messages);
        }
    }
}