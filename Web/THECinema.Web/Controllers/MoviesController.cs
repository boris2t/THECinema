namespace THECinema.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Movies;
    using THECinema.Web.ViewModels.Reviews;

    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;
        private readonly IReviewsService reviewsService;

        public MoviesController(
            IMoviesService moviesService,
            IReviewsService reviewsService)
        {
            this.moviesService = moviesService;
            this.reviewsService = reviewsService;
        }

        public IActionResult Details(int filmId)
        {
            var viewModel = this.moviesService.GetById<FullInfoMovieViewModel>(filmId);
            viewModel.Reviews = this.reviewsService.GetAllByMovieId<ReviewViewModel>(filmId);

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
