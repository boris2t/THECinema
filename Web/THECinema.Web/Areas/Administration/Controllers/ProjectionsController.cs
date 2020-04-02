namespace THECinema.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Projections;

    public class ProjectionsController : AdministrationController
    {
        private readonly IMoviesService moviesService;
        private readonly IHallsService hallsService;
        private readonly IProjectionsService projectionsService;

        public ProjectionsController(
            IMoviesService moviesService,
            IHallsService hallsService,
            IProjectionsService projectionsService)
        {
            this.moviesService = moviesService;
            this.hallsService = hallsService;
            this.projectionsService = projectionsService;
        }

        public IActionResult Add()
        {
            var movies = this.moviesService.GetAll<MovieDropDownViewModel>(null);
            var halls = this.hallsService.GetAll<HallDropDownViewModel>();
            var viewModel = new AddProjectionInputModel
            {
                HallId = 1,
                Movies = movies,
                Halls = halls,
                ProjectionDateTime = new DateTime(DateTime.Now.Ticks / 600000000 * 600000000),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectionInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.projectionsService.AddAsync(inputModel);

            return this.Redirect("/");
        }
    }
}
