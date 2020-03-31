namespace CinemaSystem.Web.ViewModels.Reservations
{
    using System.Collections.Generic;

    using CinemaSystem.Data.Models;
    using CinemaSystem.Web.ViewModels.Halls;
    using CinemaSystem.Web.ViewModels.Movies;

    public class ReservationViewModel
    {
        public Dictionary<string, List<ProjectionSeat>> Seats { get; set; }

        public string ProjectionId { get; set; }

        public ReservationMovieViewModel Movie { get; set; }

        public ReservationHallViewModel Hall { get; set; }

        public double Price { get; set; }
    }
}
