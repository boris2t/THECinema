namespace CinemaSystem.Web.ViewModels.Projections
{
    using System;

    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class ProjectionViewModel : IMapFrom<Projection>
    {
        public string Id { get; set; }

        public Hall Hall { get; set; }

        public Movie Movie { get; set; }

        public string ProjectionType { get; set; }

        public DateTime ProjectionDateTime { get; set; }
    }
}
