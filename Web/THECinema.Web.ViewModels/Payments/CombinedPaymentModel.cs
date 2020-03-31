namespace THECinema.Web.ViewModels.Payments
{
    using THECinema.Web.ViewModels.Reservations;

    public class CombinedPaymentModel
    {
        public PaymentTypeInputModel InputModel { get; set; }

        public FullInfoReservationViewModel ViewModel { get; set; }
    }
}
