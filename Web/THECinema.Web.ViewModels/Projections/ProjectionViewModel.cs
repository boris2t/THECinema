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
            switch (this.ProjectionType)
            {
                case "TwoD":
                    this.Type = "2D";
                    break;
                case "ThreeD":
                    this.Type = "3D";
                    break;
                default:
                    this.Type = "4Dx";
                    break;
            }
        }

        public string Id { get; set; }

        public Hall Hall { get; set; }

        public Movie Movie { get; set; }

        public string ProjectionType { get; set; }

        public string Type { get; set; }

        public DateTime ProjectionDateTime { get; set; }
    }
}
