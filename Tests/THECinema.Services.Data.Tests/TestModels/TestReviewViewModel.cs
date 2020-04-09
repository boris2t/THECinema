namespace THECinema.Services.Data.Tests.TestModels
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class TestReviewViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public double Stars { get; set; }
    }
}
