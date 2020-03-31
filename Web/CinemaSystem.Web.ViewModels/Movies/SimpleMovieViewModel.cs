namespace CinemaSystem.Web.ViewModels.Movies
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class SimpleMovieViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TrailerUrl { get; set; }
    }
}
