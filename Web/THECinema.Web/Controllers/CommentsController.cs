namespace THECinema.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Comments;

    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(
            ICommentsService commentsService,
            UserManager<ApplicationUser> userManager)
        {
            this.commentsService = commentsService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCommentInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            inputModel.ApplicationUserId = user.Id;

            await this.commentsService.AddAsync(inputModel);
            return this.RedirectToAction("Details", "Movies", new { filmId = inputModel.MovieId });
        }

        public async Task<IActionResult> Delete(int id, int movieId)
        {
            await this.commentsService.DeleteAsync(id);
            return this.RedirectToAction("Details", "Movies", new { filmId = movieId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddCommentInputModel inputModel)
        {
            await this.commentsService.EditAsync(inputModel);

            return this.RedirectToAction("Details", "Movies", new { filmId = inputModel.MovieId });
        }
    }
}
