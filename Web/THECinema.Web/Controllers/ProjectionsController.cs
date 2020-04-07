namespace THECinema.Web.Controllers
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Projections;

    public class ProjectionsController : BaseController
    {
        private readonly IProjectionsService projectionsService;

        public ProjectionsController(IProjectionsService projectionsService)
        {
            this.projectionsService = projectionsService;
        }

        public IActionResult GetById(int filmId)
        {
            var projections = this.projectionsService
                .GetById<ProjectionViewModel>(filmId);

            var monday = projections.Where(p => p.ProjectionDateTime.DayOfWeek == DayOfWeek.Monday);
            var tuesday = projections.Where(p => p.ProjectionDateTime.DayOfWeek == DayOfWeek.Tuesday);
            var wednesday = projections.Where(p => p.ProjectionDateTime.DayOfWeek == DayOfWeek.Wednesday);
            var thursday = projections.Where(p => p.ProjectionDateTime.DayOfWeek == DayOfWeek.Thursday);
            var friday = projections.Where(p => p.ProjectionDateTime.DayOfWeek == DayOfWeek.Friday);
            var saturday = projections.Where(p => p.ProjectionDateTime.DayOfWeek == DayOfWeek.Saturday);
            var sunday = projections.Where(p => p.ProjectionDateTime.DayOfWeek == DayOfWeek.Sunday);

            var viewModel = new AllProjectionsViewModel
            {
                AllProjections = projections,
                MondayProjections = monday,
                TuesDayProjections = tuesday,
                WednesdayProjections = wednesday,
                ThursdayProjections = thursday,
                FridayProjections = friday,
                SaturdayProjections = saturday,
                SundayProjections = sunday,
            };

            return this.View(viewModel);
        }
    }
}
