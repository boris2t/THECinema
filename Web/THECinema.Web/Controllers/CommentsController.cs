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
        public async Task<ActionResult<CommentViewModel>> Add(AddCommentInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            inputModel.ApplicationUserId = user.Id;

            var viewModel = await this.commentsService.AddAsync(inputModel);
            viewModel.ApplicationUserUserName = user.UserName;
            return viewModel;
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.commentsService.DeleteAsync(id);
            return this.Ok();
        }

        [HttpPost]
        public async Task<ActionResult<CommentViewModel>> Edit(AddCommentInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = await this.commentsService.EditAsync(inputModel);
            viewModel.ApplicationUserUserName = user.UserName;
            return viewModel;
        }
    }
}
