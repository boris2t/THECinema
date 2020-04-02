namespace THECinema.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Reviews;

    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task AddAsync(AddReviewInputModel inputModel)
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
        }

        public async Task DeleteAsync(int id)
        {
            var review = this.reviewsRepository.All().Where(r => r.Id == id).FirstOrDefault();
            this.reviewsRepository.Delete(review);
            await this.reviewsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(AddReviewInputModel inputModel)
        {
            var review = await this.reviewsRepository.GetByIdWithDeletedAsync(inputModel.Id);

            review.Title = inputModel.Title;
            review.Content = inputModel.Content;

            this.reviewsRepository.Update(review);
            await this.reviewsRepository.SaveChangesAsync();
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
