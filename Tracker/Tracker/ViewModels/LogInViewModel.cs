using System.ComponentModel.DataAnnotations;


namespace Tracker.ViewModels
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "RequiredField")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "RequiredField")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember")]
        public bool RememberMe { get; set; }
    }
}
