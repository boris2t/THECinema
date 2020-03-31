namespace THECinema.Data.Models
{
    using THECinema.Data.Common.Models;
    using THECinema.Data.Models.Enums;

    public class Payment : BaseDeletableModel<string>
    {
        public PaymentType PaymentType { get; set; }

        public string ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
