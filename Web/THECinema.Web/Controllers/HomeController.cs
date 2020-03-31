namespace THECinema.Web.Controllers
{
    using System.Diagnostics;

    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels;
    using THECinema.Web.ViewModels.Movies;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IMoviesService moviesService;

        public HomeController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexSimpleMovieViewModel
            {
                Movies = this.moviesService.GetAll<SimpleMovieViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
