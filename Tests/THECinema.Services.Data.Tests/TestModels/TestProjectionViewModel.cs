namespace THECinema.Services.Data.Tests.TestModels
{
    using System;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class TestProjectionViewModel : IMapFrom<Projection>
    {
        public int MovieId { get; set; }

        public int HallId { get; set; }

        public DateTime ProjectionDateTime { get; set; }
    }
}
