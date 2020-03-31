namespace THECinema.Web.ViewModels.Movies
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class SimpleMovieViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TrailerUrl { get; set; }
    }
}
