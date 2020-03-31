namespace CinemaSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaSystem.Services.Data.Contracts;
    using CinemaSystem.Web.ViewModels.Halls;
    using CinemaSystem.Web.ViewModels.Managements;
    using CinemaSystem.Web.ViewModels.Movies;
    using CinemaSystem.Web.ViewModels.Projections;
    using Microsoft.AspNetCore.Mvc;

    public class ManagementsController : AdministrationController
    {
        private readonly IMoviesService moviesService;
        private readonly IHallsService hallsService;
        private readonly IProjectionsService projectionsService;

        public ManagementsController(
            IMoviesService moviesService,
            IHallsService hallsService,
            IProjectionsService projectionsService)
        {
            this.moviesService = moviesService;
            this.hallsService = hallsService;
            this.projectionsService = projectionsService;
        }

        public IActionResult Manage()
        {
            var viewModel = new ManagementViewModel
            {
                Movies = this.moviesService.GetAll<SimpleMovieViewModel>(),
                Halls = this.hallsService.GetAll<ReservationHallViewModel>(),
                Projections = this.projectionsService.GetAll<ProjectionViewModel>(),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> DeleteMovie(int filmId)
        {
            await this.moviesService.DeleteAsync(filmId);

            return this.RedirectToAction("Manage");
        }

        public async Task<IActionResult> DeleteHall(int id)
        {
            await this.hallsService.DeleteAsync(id);

            return this.RedirectToAction("Manage");
        }

        public async Task<IActionResult> DeleteProjection(string id)
        {
            await this.projectionsService.DeleteAsync(id);

            return this.RedirectToAction("Manage");
        }

        public IActionResult EditMovie(int filmId)
        {
            var viewModel = this.moviesService.GetById<AddMovieInputModel>(filmId);
            viewModel.Id = filmId;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(AddMovieInputModel inputModel)
        {
            await this.moviesService.EditAsync(inputModel);

            return this.Redirect($"/Movies/Details?filmId={inputModel.Id}");
        }

        public IActionResult EditProjection(string id)
        {
            var viewModel = this.projectionsService.GetByProjectionId<AddProjectionInputModel>(id);
            viewModel.ProjectionDateTime = viewModel.ProjectionDateTime.ToLocalTime();
            viewModel.Movies = this.moviesService.GetAll<MovieDropDownViewModel>();
            viewModel.Halls = this.hallsService.GetAll<HallDropDownViewModel>();
            viewModel.Id = id;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProjection(AddProjectionInputModel inputModel)
        {
            await this.projectionsService.EditAsync(inputModel);

            return this.RedirectToAction("Manage");
        }
    }
}
