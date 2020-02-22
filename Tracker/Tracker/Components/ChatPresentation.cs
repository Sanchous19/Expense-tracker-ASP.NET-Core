using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace Tracker.Components
{
    public class ChatPresentation : ViewComponent
    {
        public IViewComponentResult Invoke(Chat chat)
        {
            Message lastMessage = chat.Messages.OrderBy(m => m.Date).Last();
            ViewBag.Text = lastMessage.Text;
            ViewBag.AuthorName = lastMessage.UserFrom.UserName;
            return View("Chat", chat);
        }
    }
}
