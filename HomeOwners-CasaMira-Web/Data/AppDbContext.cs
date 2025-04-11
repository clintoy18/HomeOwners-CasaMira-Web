using HomeOwners_CasaMira_Web.Controllers;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HomeOwners_CasaMira_Web.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options){ }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<FacilityReservation> FacilityReservations { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Facility> Facilities { get; set; }

        public static implicit operator AppDbContext(AppbContext v)
        {
            throw new NotImplementedException();
        }
    }
}
