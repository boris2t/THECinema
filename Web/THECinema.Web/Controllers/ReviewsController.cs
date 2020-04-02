namespace THECinema.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Reviews;

    public class ReviewsController : BaseController
    {
        private readonly IReviewsService reviewsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewsController(
            IReviewsService reviewsService,
            UserManager<ApplicationUser> userManager)
        {
            this.reviewsService = reviewsService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddReviewInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            inputModel.ApplicationUserId = user.Id;

            await this.reviewsService.AddAsync(inputModel);
            return this.RedirectToAction("Details", "Movies", new { filmId = inputModel.MovieId });
        }

        public async Task<IActionResult> Delete(int id, int movieId)
        {
            await this.reviewsService.DeleteAsync(id);
            return this.RedirectToAction("Details", "Movies", new { filmId = movieId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddReviewInputModel inputModel)
        {
            await this.reviewsService.EditAsync(inputModel);

            return this.RedirectToAction("Details", "Movies", new { filmId = inputModel.MovieId });
        }
    }
}
