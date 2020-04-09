namespace THECinema.Web.ViewModels.Projections
{
    using System;
    using System.Globalization;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class ProjectionViewModel : IMapFrom<Projection>
    {
        public ProjectionViewModel()
        {
            this.Type = this.ProjectionType switch
            {
                "TwoD" => "2D",
                "ThreeD" => "3D",
                _ => "4Dx",
            };
        }

        public string Id { get; set; }

        public int HallId { get; set; }

        public Hall Hall { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public string ProjectionType { get; set; }

        public string Type { get; set; }

        public DateTime ProjectionDateTime { get; set; }
    }
}
