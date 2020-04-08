namespace THECinema.Services.Data.Tests
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class TestMovieViewModel : IMapFrom<Movie>
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public double Rating { get; set; }
    }
}