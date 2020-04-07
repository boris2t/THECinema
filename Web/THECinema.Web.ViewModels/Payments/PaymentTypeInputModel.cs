namespace THECinema.Web.ViewModels.Payments
{
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class PaymentTypeInputModel : IMapTo<Payment>
    {
        [Required]
        public string PaymentType { get; set; }

        [Required]
        public string ReservationId { get; set; }
    }
}
