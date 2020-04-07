namespace THECinema.Web.ViewModels.Projections
{
    using System.Collections.Generic;

    public class AllProjectionsViewModel
    {
        public IEnumerable<ProjectionViewModel> AllProjections { get; set; }

        public IEnumerable<ProjectionViewModel> MondayProjections { get; set; }

        public IEnumerable<ProjectionViewModel> TuesDayProjections { get; set; }

        public IEnumerable<ProjectionViewModel> WednesdayProjections { get; set; }

        public IEnumerable<ProjectionViewModel> ThursdayProjections { get; set; }

        public IEnumerable<ProjectionViewModel> FridayProjections { get; set; }

        public IEnumerable<ProjectionViewModel> SaturdayProjections { get; set; }

        public IEnumerable<ProjectionViewModel> SundayProjections { get; set; }
    }
}
