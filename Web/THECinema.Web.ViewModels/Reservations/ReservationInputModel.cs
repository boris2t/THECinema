namespace THECinema.Web.ViewModels.Reservations
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class ReservationInputModel : IMapTo<Reservation>
    {
        public string ProjectionId { get; set; }

        public string SelectedSeats { get; set; }

        public string Price { get; set; }
    }
}
