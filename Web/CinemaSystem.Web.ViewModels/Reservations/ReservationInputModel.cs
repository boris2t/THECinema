namespace CinemaSystem.Web.ViewModels.Reservations
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class ReservationInputModel : IMapTo<Reservation>
    {
        public string ProjectionId { get; set; }

        public string SelectedSeats { get; set; }

        public string Price { get; set; }
    }
}
