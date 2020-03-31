namespace THECinema.Web.ViewModels.Payments
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class PaymentTypeInputModel : IMapTo<Payment>
    {
        public string PaymentType { get; set; }

        public string ReservationId { get; set; }
    }
}
