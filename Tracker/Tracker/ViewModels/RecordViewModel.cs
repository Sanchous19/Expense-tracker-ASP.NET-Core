using System;
using System.ComponentModel.DataAnnotations;


namespace Tracker.ViewModels
{
    public class RecordViewModel
    {
        [Required(ErrorMessage = "RequiredField")]
        [Display(Name = "PurchaseThing")]
        public string Thing { get; set; }

        [Required(ErrorMessage = "RequiredField")]
        [Display(Name = "Cost")]
        public double Cost { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "RequiredField")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public string ReturnUrl { get; set; }
    }
}
