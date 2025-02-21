using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeOwners_CasaMira_Web.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

    }

}
