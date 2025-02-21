using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.ViewModels
{
    public class RegisterViewModel
    {
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [EmailAddress]
    public string Email { get; set; }


    [Required(ErrorMessage = "Password is required.")]
    [StringLength(40 , ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
    public string Password { get; set; }


    [Required(ErrorMessage = "Confirm Password is Required. ")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }



    }
}
