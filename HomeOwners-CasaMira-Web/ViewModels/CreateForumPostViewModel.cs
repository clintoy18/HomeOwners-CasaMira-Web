using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.ViewModels
{
    public class CreateForumPostViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [Display(Name = "Content")]
        public string Content { get; set; }
    }
} 