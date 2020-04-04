namespace THECinema.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        Task<CommentViewModel> AddAsync(AddCommentInputModel inputModel);

        Task DeleteAsync(int id);

        Task EditAsync(AddCommentInputModel inputModel);
    }
}
