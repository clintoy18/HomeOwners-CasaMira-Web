using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Users : IdentityUser
    {
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }  // Change to DateTime? if optional

        public string Role { get; set; } // Consider using Identity Roles instead
    }
}
