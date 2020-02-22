using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker.Models
{
    public class Record
    {
        public Record(string thing, double cost, DateTime date, string description = "")
        {
            Thing = thing;
            Cost = cost;
            Date = date;
            Description = description;
        }

        public Record(User owner, string thing, double cost, DateTime date, string description = "")
            : this(thing, cost, date, description)
        {
            Owner = owner;
        }

        public string Id { get; set; }
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public string Thing { get; set; }
        public double Cost { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        private class Comparer : IComparer<Record>
        {
            public int Compare(Record r1, Record r2) => r2.Date.CompareTo(r1.Date);
        }

        public static List<List<Record>> GroupByMonth(List<Record> records)
        {
            List<List<Record>> result = new List<List<Record>>();
            records.Sort(new Comparer());
            foreach (var record in records)
            {
                if (result.Count != 0 && record.Date.Year == result[result.Count - 1][0].Date.Year && record.Date.Month == result[result.Count - 1][0].Date.Month)
                {
                    result[result.Count - 1].Add(record);
                }
                else
                {
                    result.Add(new List<Record>() { record });
                }
            }
            return result;
        }

        public static List<List<Record>> GroupByDay(List<Record> records)
        {
            List<List<Record>> result = new List<List<Record>>();
            foreach (var record in records)
            {
                if (result.Count != 0 && record.Date.Day == result[result.Count - 1][0].Date.Day)
                {
                    result[result.Count - 1].Add(record);
                }
                else
                {
                    result.Add(new List<Record>() { record });
                }
            }
            return result;
        }
    }
}
