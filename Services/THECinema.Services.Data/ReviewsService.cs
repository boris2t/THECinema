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
