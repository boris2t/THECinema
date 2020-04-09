namespace THECinema.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        Task<CommentViewModel> AddAsync(AddCommentInputModel inputModel);

        Task DeleteAsync(int id);

        Task<CommentViewModel> EditAsync(AddCommentInputModel inputModel);

        IEnumerable<int> GetByReviewId(int reviewId);
    }
}
