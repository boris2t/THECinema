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
        public async Task<ActionResult<ReviewViewModel>> Add(AddReviewInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            inputModel.ApplicationUserId = user.Id;

            var viewModel = await this.reviewsService.AddAsync(inputModel);
            viewModel.ApplicationUserUserName = user.UserName;
            return viewModel;
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.reviewsService.DeleteAsync(id);
            return this.Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ReviewViewModel>> Edit(AddReviewInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = await this.reviewsService.EditAsync(inputModel);
            viewModel.ApplicationUserUserName = user.UserName;
            return viewModel;
        }
    }
}
