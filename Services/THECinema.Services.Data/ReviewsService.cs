namespace THECinema.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Comments;
    using THECinema.Web.ViewModels.Reviews;

    public class ReviewsService : IReviewsService
    {
        private const string InvalidIdExceptionMessage = "Review doesn't exist!";

        private readonly IDeletableEntityRepository<Review> reviewsRepository;
        private readonly ICommentsService commentsService;

        public ReviewsService(
            IDeletableEntityRepository<Review> reviewsRepository,
            ICommentsService commentsService)
        {
            this.reviewsRepository = reviewsRepository;
            this.commentsService = commentsService;
        }

        public async Task<ReviewViewModel> AddAsync(AddReviewInputModel inputModel)
        {
            var review = new Review
            {
                ApplicationUserId = inputModel.ApplicationUserId,
                MovieId = inputModel.MovieId,
                Title = inputModel.Title,
                Content = inputModel.Content,
                Stars = inputModel.Stars,
            };

            await this.reviewsRepository.AddAsync(review);
            await this.reviewsRepository.SaveChangesAsync();

            var viewModel = new ReviewViewModel
            {
                Id = review.Id,
                Title = review.Title,
                Content = review.Content,
                Stars = review.Stars,
                CreatedOn = review.CreatedOn,
            };

            return viewModel;
        }

        public async Task<CommentsForDeleteViewModel> DeleteAsync(int id)
        {
            var review = this.reviewsRepository.All().Where(r => r.Id == id).FirstOrDefault();

            if (review == null)
            {
                throw new ArgumentNullException(InvalidIdExceptionMessage);
            }

            var commentIds = this.commentsService.GetByReviewId(id);

            foreach (var commentId in commentIds)
            {
                await this.commentsService.DeleteAsync(commentId);
            }

            this.reviewsRepository.Delete(review);
            await this.reviewsRepository.SaveChangesAsync();

            var viewModel = new CommentsForDeleteViewModel
            {
                CommentIds = commentIds,
            };

            return viewModel;
        }

        public async Task<ReviewViewModel> EditAsync(AddReviewInputModel inputModel)
        {
            var review = await this.reviewsRepository.GetByIdWithDeletedAsync(inputModel.Id);

            if (review == null)
            {
                throw new ArgumentNullException(InvalidIdExceptionMessage);
            }

            review.Title = inputModel.Title;
            review.Content = inputModel.Content;

            this.reviewsRepository.Update(review);
            await this.reviewsRepository.SaveChangesAsync();

            var viewModel = new ReviewViewModel
            {
                Id = review.Id,
                Title = review.Title,
                Content = review.Content,
                CreatedOn = review.CreatedOn,
                Stars = review.Stars,
            };

            return viewModel;
        }

        public IEnumerable<T> GetAllByMovieId<T>(int movieId)
        {
            return this.reviewsRepository
                .All()
                .Where(r => r.MovieId == movieId)
                .To<T>()
                .ToList();
        }
    }
}
