namespace THECinema.Web.Controllers
{
    using System.Linq;

    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Projections;
    using Microsoft.AspNetCore.Mvc;

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

            var viewModel = new AllProjectionsViewModel
            {
                Projections = projections.OrderBy(p => p.ProjectionDateTime),
            };

            return this.View(viewModel);
        }
    }
}
