using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tracker.Utils;


namespace Tracker.Models
{
    public enum UserRole
    {
        Admin,
        Moderator,
        BlockedUser,
    }


    public class User : IdentityUser
    {
        public User()
        {
            Records = new List<Record>();
            CommonChats = new ChatList();
        }
        
        public List<Record> Records { get; set; }
        public string FamilyChatId { get; set; }
        public Chat FamilyChat { get; set; }
        public string CommonChatsId { get; set; }
        public ChatList CommonChats { get; set; }
    }
}
