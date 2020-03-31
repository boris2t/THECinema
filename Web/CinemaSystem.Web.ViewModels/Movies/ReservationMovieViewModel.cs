namespace CinemaSystem.Web.ViewModels.Movies
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class ReservationMovieViewModel : IMapFrom<Movie>
    {
        public string Name { get; set; }

        public double Price { get; set; }
    }
}
