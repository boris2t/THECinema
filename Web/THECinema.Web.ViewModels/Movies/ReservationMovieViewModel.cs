namespace THECinema.Web.ViewModels.Movies
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class ReservationMovieViewModel : IMapFrom<Movie>
    {
        public string Name { get; set; }

        public double Price { get; set; }
    }
}
