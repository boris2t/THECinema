namespace THECinema.Web.ViewModels.Projections
{
    using System;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class ProjectionViewModel : IMapFrom<Projection>
    {
        public string Id { get; set; }

        public Hall Hall { get; set; }

        public Movie Movie { get; set; }

        public string ProjectionType { get; set; }

        public DateTime ProjectionDateTime { get; set; }
    }
}
