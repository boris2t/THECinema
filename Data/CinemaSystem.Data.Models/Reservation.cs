namespace CinemaSystem.Data.Models
{
    using System.Collections.Generic;

    using CinemaSystem.Data.Common.Models;

    public class Reservation : BaseDeletableModel<string>
    {
        public Reservation()
        {
            this.Seats = new HashSet<ProjectionSeat>();
        }

        public string ProjectionId { get; set; }

        public virtual Projection Projection { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string SelectedSeats { get; set; }

        public double Price { get; set; }

        public IEnumerable<ProjectionSeat> Seats { get; set; }
    }
}
