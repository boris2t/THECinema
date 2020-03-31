namespace CinemaSystem.Web.ViewModels.Payments
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class PaymentTypeInputModel : IMapTo<Payment>
    {
        public string PaymentType { get; set; }

        public string ReservationId { get; set; }
    }
}
