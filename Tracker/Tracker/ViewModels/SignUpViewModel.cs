using System.ComponentModel.DataAnnotations;


namespace Tracker.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "RequiredField")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "RequiredField")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RequiredField")]
        [Compare("Password", ErrorMessage = "PasswordsDoNotMatch")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string PasswordConfirm { get; set; }
    }
}
