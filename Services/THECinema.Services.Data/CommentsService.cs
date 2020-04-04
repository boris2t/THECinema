namespace THECinema.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Comments;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task<CommentViewModel> AddAsync(AddCommentInputModel inputModel)
        {
            var comment = new Comment
            {
                ApplicationUserId = inputModel.ApplicationUserId,
                Content = inputModel.Content,
                ReviewId = inputModel.ReviewId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();

            var viewModel = new CommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
            };

            return viewModel;
        }

        public async Task DeleteAsync(int id)
        {
            var comment = this.commentsRepository.All().Where(c => c.Id == id).FirstOrDefault();
            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(AddCommentInputModel inputModel)
        {
            var comment = await this.commentsRepository.GetByIdWithDeletedAsync(inputModel.Id);
            comment.Content = inputModel.Content;

            this.commentsRepository.Update(comment);
            await this.commentsRepository.SaveChangesAsync();
        }
    }
}
