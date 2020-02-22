using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Tracker.Models
{
    public class RecordCollection
    {
        public RecordCollection()
        {
            Dates = new List<DateTime>();
            Records = new SortedList<DateTime, SortedList<DateTime, List<Record>>>(new ReversedComparer());
        }
        
        [NotMapped]
        public List<DateTime> Dates { get; set; }
        [NotMapped]
        public SortedList<DateTime, SortedList<DateTime, List<Record>>> Records { get; set; }

        public void Append(Record record)
        {
            DateTime date = record.Date;
            DateTime dateKey = new DateTime(date.Year, date.Month, 1);
            if (Records.ContainsKey(dateKey))
            {
                if (Records[dateKey].ContainsKey(date))
                {
                    Records[dateKey][date].Add(record);
                }
                else
                {
                    Records[dateKey].Add(date, new List<Record>() { record });
                }
            }
            else
            {
                Records.Add(dateKey, new SortedList<DateTime, List<Record>>(new ReversedComparer()));
                Records[dateKey].Add(date, new List<Record>() { record });
                InsertDate(dateKey);
            }
        }

        public (DateTime, SortedList<DateTime, List<Record>>) Get(int index)
        {
            DateTime dateKey = Dates[index];
            return (dateKey, Records[dateKey]);
        }

        public bool Empty()
        {
            return Dates.Count == 0;
        }

        private void InsertDate(DateTime date)
        {
            int index = Binpow(date, 0, Dates.Count);
            Dates.Insert(index, date);
        }

        private int Binpow(DateTime date, int l, int r)
        {
            if (l == r)
                return l;
            int m = (l + r) / 2;
            if (date >= Dates[m])
                return Binpow(date, l, m);
            else
                return Binpow(date, m + 1, r);
        }

        private class ReversedComparer : IComparer<DateTime>
        {
            public int Compare(DateTime d1, DateTime d2) => d2.CompareTo(d1);
        }
    }
}
