namespace THECinema.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using THECinema.Common;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Messaging;
    using THECinema.Web.ViewModels;
    using THECinema.Web.ViewModels.Home;
    using THECinema.Web.ViewModels.Movies;

    public class HomeController : BaseController
    {
        private const int MoviesPerPage = 10;

        private readonly IMoviesService moviesService;
        private readonly IEmailSender emailSender;

        public HomeController(
            IMoviesService moviesService,
            IEmailSender emailSender)
        {
            this.moviesService = moviesService;
            this.emailSender = emailSender;
        }

        public IActionResult Index(int page = 1)
        {
            var viewModel = new IndexSimpleMovieViewModel
            {
                Movies = this.moviesService.GetAll<SimpleMovieViewModel>(MoviesPerPage, (page - 1) * MoviesPerPage),
            };

            viewModel.CurrentPage = page;
            var count = this.moviesService.GetMoviesCount();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / MoviesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
           return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactUsInputModel inputModel)
        {
            await this.emailSender.SendEmailAsync(
                inputModel.Email,
                inputModel.Name,
                GlobalConstants.SystemEmail,
                inputModel.Subject,
                inputModel.Message);

            return this.View("Sent");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            var viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                StatusCode = statusCode,
            };

            if (statusCode == StatusCodes.Status404NotFound)
            {
                return this.View("404", viewModel);
            }
            else
            {
                return this.View(viewModel);
            }
        }
    }
}
