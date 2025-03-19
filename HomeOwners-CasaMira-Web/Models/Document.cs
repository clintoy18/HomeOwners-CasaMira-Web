using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string FilePath { get; set; } // Path to uploaded document

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }

}
