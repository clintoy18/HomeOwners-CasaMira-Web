using Microsoft.AspNetCore.Identity;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }

        public static implicit operator Users(Users v)
        {
            throw new NotImplementedException();
        }
    }
}
