namespace THECinema.Web.ViewModels.Reservations
{
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class ReservationInputModel : IMapTo<Reservation>
    {
        [Required]
        public string ProjectionId { get; set; }

        [Required]
        public string SelectedSeats { get; set; }

        public string Price { get; set; }
    }
}
