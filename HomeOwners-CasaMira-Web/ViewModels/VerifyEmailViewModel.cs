using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.ViewModels
{
    public class VerifyEmailViewModel
    {

        [Required(ErrorMessage = "Email i required. ")]
        [EmailAddress]

        public string Email { get; set; }
    }
}
