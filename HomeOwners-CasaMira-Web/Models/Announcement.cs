using System;
using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Use UTC for consistency
        public DateTime DatePosted { get; set; } = DateTime.UtcNow; // Make it DateTime, not object
    }
}
