namespace CinemaSystem.Web.ViewModels.Movies
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class FullInfoMovieViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }

        public string Director { get; set; }

        public string TrailerUrl { get; set; }

        public string Actors { get; set; }

        public int AgeRestriction { get; set; }

        public int Duration { get; set; }

        public double Price { get; set; }

        public string Genre { get; set; }
    }
}
