namespace THECinema.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using THECinema.Data;
    using THECinema.Data.Models;
    using THECinema.Data.Repositories;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Data.Tests.TestModels;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Reviews;
    using Xunit;

    public class ReviewsServiceTests
    {
        private readonly AddReviewInputModel review;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> options;

        public ReviewsServiceTests()
        {
            this.review = new AddReviewInputModel
            {
                ApplicationUserId = "userId",
                MovieId = 1,
                Title = "Cool!",
                Content = "Nice!",
                Stars = 4,
            };

            AutoMapperConfig.RegisterMappings(typeof(TestReviewViewModel).Assembly);
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task AddReviewShouldAddCorrectCount()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            await service.AddAsync(this.review);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddReviewShouldAddCorrectData()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            var result = await service.AddAsync(this.review);
            Assert.Equal("Cool!", result.Title);
            Assert.Equal(4, result.Stars);
        }

        [Fact]
        public async Task DeleteReviewShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            var added = await service.AddAsync(this.review);
            await service.DeleteAsync(added.Id);
            var deleted = await repository.GetByIdWithDeletedAsync(added.Id);

            Assert.True(deleted.IsDeleted);
        }

        [Fact]
        public void DeleteReviewShouldThrowIfInvalidId()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            Assert.Throws<ArgumentNullException>(() => service.DeleteAsync(2).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task EditReviewShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            var diffReview = new AddReviewInputModel
            {
                Id = 1,
                Title = "title",
                Content = "content",
            };

            var added = await service.AddAsync(this.review);

            var result = await service.EditAsync(diffReview);

            Assert.Equal("title", result.Title);
            Assert.Equal("content", result.Content);
        }

        [Fact]
        public void EditReviewShouldThrowIfIdIsInvalid()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            Assert.Throws<ArgumentNullException>(() => service.EditAsync(this.review).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAllByMovieIdShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            var diffReview = new AddReviewInputModel
            {
                ApplicationUserId = "me",
                MovieId = 2,
                Title = "title",
                Content = "content",
                Stars = 3,
            };

            await service.AddAsync(diffReview);
            await service.AddAsync(this.review);
            await service.AddAsync(this.review);

            var result = service.GetAllByMovieId<TestReviewViewModel>(1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllByMovieIdShouldReturnEmptyListWithInvalidMovieId()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Review>(context);
            var service = this.GetReviewsService(repository);

            var diffReview = new AddReviewInputModel
            {
                ApplicationUserId = "me",
                MovieId = 2,
                Title = "title",
                Content = "content",
                Stars = 3,
            };

            await service.AddAsync(diffReview);
            await service.AddAsync(this.review);

            var result = service.GetAllByMovieId<TestReviewViewModel>(3);

            Assert.Empty(result);
        }

        private ReviewsService GetReviewsService(
           EfDeletableEntityRepository<Review> reviewsRepository)
        {
            var commentsServiceMock = new Mock<ICommentsService>();

            var reviewsService = new ReviewsService(
                reviewsRepository,
                commentsServiceMock.Object);

            return reviewsService;
        }
    }
}
