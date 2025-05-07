using HomeOwners_CasaMira_Web.Controllers;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HomeOwners_CasaMira_Web.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<FacilityReservation> FacilityReservations { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ServiceRequest entity
            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.Location)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.StaffNotes)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.Status)
                .IsRequired()
                .HasDefaultValue("Pending");

            // Configure ForumPost entity
            modelBuilder.Entity<ForumPost>()
                .Property(f => f.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<ForumPost>()
                .Property(f => f.Content)
                .IsRequired();

            // Configure relationships
            modelBuilder.Entity<ForumPost>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.ForumPost)
                .HasForeignKey(c => c.ForumPostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ForumComment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ForumPost>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
