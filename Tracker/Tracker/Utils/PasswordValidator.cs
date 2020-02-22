using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Tracker.Models;


namespace Tracker.Utils
{
    public class PasswordValidator : IPasswordValidator<User>
    {
        private readonly IStringLocalizer<PasswordValidator> _localizer;
        
        public PasswordValidator(IStringLocalizer<PasswordValidator> localizer, int length)
        {
            MinimumLength = length;
            _localizer = localizer;
        }

        public int MinimumLength { get; set; }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            string contain_letter = "(?=.*[a-zA-Z])",  contain_number = "(?=.*\\d)", pattern = "^[A-Za-z0-9]+$";
            List<IdentityError> errors = new List<IdentityError>();

            if (password.Length < MinimumLength)
            {
                errors.Add(new IdentityError
                {
                    Description = string.Format(_localizer["TheMinimumPasswordLengthIs"], MinimumLength)
                });
            }
            if (!Regex.IsMatch(password, contain_letter))
            {
                errors.Add(new IdentityError
                {
                    Description = _localizer["PasswordMustContainAtLeastOneLatinLetter"]
                });
            }
            if (!Regex.IsMatch(password, contain_number))
            {
                errors.Add(new IdentityError
                {
                    Description = _localizer["PasswordMustContainAtLeastOneNumber"]
                });
            }
            if (!Regex.IsMatch(password, pattern))
            {
                errors.Add(new IdentityError
                {
                    Description = _localizer["PasswordContainsInvalidCharacters"]
                });
            }

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
