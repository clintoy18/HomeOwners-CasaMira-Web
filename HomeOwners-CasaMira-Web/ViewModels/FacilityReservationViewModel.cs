using HomeOwners_CasaMira_Web.Models;

namespace HomeOwners_CasaMira_Web.ViewModels
{
    public class FacilityReservationViewModel
    {
        public int FacilityId { get; set; }
        public List<Facility> Facilities { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
