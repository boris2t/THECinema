namespace THECinema.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Movies;

    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public IActionResult Details(int filmId)
        {
            var viewModel = this.moviesService.GetById<FullInfoMovieViewModel>(filmId);

            return this.View(viewModel);
        }

        public IActionResult Search(string filmName)
        {
            var id = this.moviesService.GetIdByName(filmName);

            if (id == 0)
            {
                return this.View("Error");
            }

            return this.RedirectToAction("Details", new { filmId = id });
        }
    }
}
