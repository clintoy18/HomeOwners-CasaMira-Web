using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Facility
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        [StringLength(255)]
        public string ImageUrl { get; set; }  // New column for image URL
    }


}
