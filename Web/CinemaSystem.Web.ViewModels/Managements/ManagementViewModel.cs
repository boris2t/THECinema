namespace CinemaSystem.Web.ViewModels.Managements
{
    using System.Collections.Generic;

    using CinemaSystem.Web.ViewModels.Halls;
    using CinemaSystem.Web.ViewModels.Movies;
    using CinemaSystem.Web.ViewModels.Projections;

    public class ManagementViewModel
    {
        public IEnumerable<SimpleMovieViewModel> Movies { get; set; }

        public IEnumerable<ReservationHallViewModel> Halls { get; set; }

        public IEnumerable<ProjectionViewModel> Projections { get; set; }
    }
}
