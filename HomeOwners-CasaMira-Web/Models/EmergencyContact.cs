using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.Models
{
    public class EmergencyContact
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; } // e.g., Police, Fire, Medical
        [Required]
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }
} 