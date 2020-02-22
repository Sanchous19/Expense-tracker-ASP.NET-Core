using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker.Models
{
    public class ChatList
    {
        public ChatList()
        {
            Chats = new List<Chat>();
        }

        public string Id { get; set; }
        public User Owner { get; set; }
        public List<Chat> Chats { get; set; }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        public IEnumerator<Chat> GetEnumerator()
        {
            Chats.Sort(new Comparer());
            return Chats.GetEnumerator();
        }

        private class Comparer : IComparer<Chat>
        {
            public int Compare(Chat c1, Chat c2) => c2.DateTimeUpdate.CompareTo(c1.DateTimeUpdate);
        }
    }
}
