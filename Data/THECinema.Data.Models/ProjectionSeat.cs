namespace THECinema.Data.Models
{
    using THECinema.Data.Common.Models;

    public class ProjectionSeat : BaseDeletableModel<string>
    {
        public string ProjectionId { get; set; }

        public virtual Projection Projection { get; set; }

        public int SeatId { get; set; }

        public virtual Seat Seat { get; set; }

        public string ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }

        public bool IsTaken { get; set; }
    }
}
