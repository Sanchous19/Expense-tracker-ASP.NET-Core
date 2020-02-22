using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Tracker.Models;


namespace Tracker.TagHelpers
{
    public class DayExpensesTagHelper : TagHelper
    {
        public List<Record> Records { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            double expenses = Records.Sum(r => r.Cost);

            output.TagName = "span";
            output.Content.SetContent(expenses.ToString());
        }
    }
}
