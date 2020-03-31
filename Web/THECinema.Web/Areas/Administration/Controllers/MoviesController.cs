namespace THECinema.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Movies;
    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : AdministrationController
    {
        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMovieInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.moviesService.AddAsync(inputModel);

            return this.Redirect("/");
        }
    }
}
