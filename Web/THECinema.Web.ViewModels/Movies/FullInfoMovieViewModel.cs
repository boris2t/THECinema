namespace THECinema.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Reviews;

    public class FullInfoMovieViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }

        public string Director { get; set; }

        public string TrailerUrl { get; set; }

        public string TrailerVideoUrl { get; set; }

        public string Actors { get; set; }

        public int AgeRestriction { get; set; }

        public int Duration { get; set; }

        public double Price { get; set; }

        public string Genre { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
