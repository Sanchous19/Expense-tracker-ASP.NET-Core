using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Tracker.Models;


namespace Tracker.TagHelpers
{
    public class MonthExpensesTagHelper : TagHelper
    {
        public List<List<Record>> Records { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            double expenses = 0;
            foreach (var dayRecords in Records)
            {
                expenses += dayRecords.Sum(r => r.Cost);
            }

            output.TagName = "span";
            output.Content.SetContent(expenses.ToString());
        }
    }
}
