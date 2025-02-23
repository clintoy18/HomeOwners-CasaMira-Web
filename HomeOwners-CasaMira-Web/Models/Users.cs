using Microsoft.AspNetCore.Identity;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Users : IdentityUser
    {
        public int FullName { get; set; }
    }
}
