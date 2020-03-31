namespace THECinema.Web.ViewModels.Managements
{
    using System.Collections.Generic;

    using THECinema.Web.ViewModels.Halls;
    using THECinema.Web.ViewModels.Movies;
    using THECinema.Web.ViewModels.Projections;

    public class ManagementViewModel
    {
        public IEnumerable<SimpleMovieViewModel> Movies { get; set; }

        public IEnumerable<ReservationHallViewModel> Halls { get; set; }

        public IEnumerable<ProjectionViewModel> Projections { get; set; }
    }
}
