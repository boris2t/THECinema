namespace CinemaSystem.Web.ViewModels.Payments
{
    using CinemaSystem.Web.ViewModels.Reservations;

    public class CombinedPaymentModel
    {
        public PaymentTypeInputModel InputModel { get; set; }

        public FullInfoReservationViewModel ViewModel { get; set; }
    }
}
