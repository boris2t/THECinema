namespace CinemaSystem.Web.ViewModels.Reservations
{
    using System.Collections.Generic;

    public class ParseReservationDataModel
    {
        public string SelectedSeats { get; set; }

        public IEnumerable<string> SelectedSeatsIds { get; set; }

        public double Price { get; set; }
    }
}
