using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeOwners_CasaMira_Web.Models
{public class FacilityReservation
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int FacilityId { get; set; }  // New field for the facility
    public DateTime ReservationDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
}

}
