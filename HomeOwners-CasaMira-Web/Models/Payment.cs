using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HomeOwners_CasaMira_Web.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } // Pending, Paid, Overdue

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }

}
