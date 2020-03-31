namespace CinemaSystem.Data.Models
{
    using CinemaSystem.Data.Common.Models;
    using CinemaSystem.Data.Models.Enums;

    public class Payment : BaseDeletableModel<string>
    {
        public PaymentType PaymentType { get; set; }

        public string ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
