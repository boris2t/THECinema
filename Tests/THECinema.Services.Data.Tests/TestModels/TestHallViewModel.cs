namespace THECinema.Services.Data.Tests.TestModels
{
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Services.Mapping;

    public class TestHallViewModel : IMapFrom<Hall>
    {
        public int Id { get; set; }

        public ProjectionType ProjectionType { get; set; }
    }
}
