namespace THECinema.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        Task AddAsync(AddCommentInputModel inputModel);

        Task DeleteAsync(int id);

        Task EditAsync(AddCommentInputModel inputModel);
    }
}
