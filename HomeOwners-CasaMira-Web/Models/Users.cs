using Microsoft.AspNetCore.Identity;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        // Removed the invalid implicit operator
    }
}
