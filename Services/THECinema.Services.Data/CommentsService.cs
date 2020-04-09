namespace THECinema.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Comments;

    public class CommentsService : ICommentsService
    {
        private const string InvalidIdExceptionMessage = "Comment doesn't exist!";

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

            if (comment == null)
            {
                throw new ArgumentNullException(InvalidIdExceptionMessage);
            }

            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task<CommentViewModel> EditAsync(AddCommentInputModel inputModel)
        {
            var comment = await this.commentsRepository.GetByIdWithDeletedAsync(inputModel.Id);

            if (comment == null)
            {
                throw new ArgumentNullException(InvalidIdExceptionMessage);
            }

            comment.Content = inputModel.Content;

            this.commentsRepository.Update(comment);
            await this.commentsRepository.SaveChangesAsync();

            var viewModel = new CommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
            };

            return viewModel;
        }

        public IEnumerable<int> GetByReviewId(int reviewId)
        {
            return this.commentsRepository
                .All()
                .Where(c => c.ReviewId == reviewId)
                .Select(c => c.Id)
                .ToList();
        }
    }
}
