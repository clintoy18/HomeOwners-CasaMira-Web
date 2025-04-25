using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeOwners_CasaMira_Web.Models
{
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Request Type")]
        public string RequestType { get; set; } // Example: Plumbing, Electrical, HVAC, etc.

        [Required]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Location")]
        public string Location { get; set; } // Where in the property the issue is located

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Created At")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated At")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(500)]
        [Display(Name = "Staff Notes")]
        public string StaffNotes { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}
