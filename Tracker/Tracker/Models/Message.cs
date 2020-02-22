using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker.Models
{
    public class Message
    {
        public Message()
        {
            Date = DateTime.Now;
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserFromId { get; set; }
        public User UserFrom { get; set; }

        public class Comparer : IComparer<Message>
        {
            public int Compare(Message m1, Message m2) => m1.Date.CompareTo(m2.Date);
        }
    }
}
