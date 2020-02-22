using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker.Models
{
    public class Chat
    {
        public Chat()
        {
            Members = new List<User>();
            Messages = new List<Message>();
            DateTimeUpdate = DateTime.Now;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<User> Members { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime DateTimeUpdate { get; set; }
        public string ChatListId { get; set; }
        public ChatList ChatList { get; set; }
    }
}
