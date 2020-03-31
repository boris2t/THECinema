namespace CinemaSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaSystem.Services.Data.Contracts;
    using CinemaSystem.Web.ViewModels.Movies;
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
