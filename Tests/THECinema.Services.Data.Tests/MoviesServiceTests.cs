namespace THECinema.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using THECinema.Data;
    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Data.Repositories;
    using THECinema.Services.Data.Tests.TestModels;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Movies;
    using Xunit;

    public class MoviesServiceTests
    {
        private readonly AddMovieInputModel movie;

        public MoviesServiceTests()
        {
            this.movie = new AddMovieInputModel
            {
                Name = "Taxi",
                Year = 1998,
                Actors = "Samy Naceri, Frédéric Diefenthal, Marion Cotillard",
                AgeRestriction = 0,
                Description = "funny",
                Director = "Gérard Pirès",
                Duration = 86,
                Rating = 7.1,
                Price = 8,
                Genre = "Action, Comedy, Crime",
                TrailerUrl = "url",
                TrailerVideoUrl = "url",
            };
            AutoMapperConfig.RegisterMappings(typeof(TestMovieViewModel).Assembly);
        }

        [Fact]
        public async Task AddMovieShouldAddCorrectCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddMovieShouldAddCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);

            var dbMovie = repository.All().Where(m => m.Name == "Taxi").FirstOrDefault();
            Assert.Equal("Taxi", dbMovie.Name);
            Assert.Equal(1998, dbMovie.Year);
            Assert.Equal(7.1, dbMovie.Rating);
        }

        [Fact]
        public void GetMoviesCountShouldReturn3()
        {
            var repository = new Mock<IDeletableEntityRepository<Movie>>();
            repository.Setup(r => r.All()).Returns(new List<Movie>
                                                        {
                                                            new Movie(),
                                                            new Movie(),
                                                            new Movie(),
                                                        }.AsQueryable());
            var service = new MoviesService(repository.Object);
            Assert.Equal(3, service.GetMoviesCount());
            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void GetMoviesCountShouldReturn0()
        {
            var repository = new Mock<IDeletableEntityRepository<Movie>>();
            var service = new MoviesService(repository.Object);
            Assert.Equal(0, service.GetMoviesCount());
            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task GetIdByNameShouldReturnTheCorrectId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);

            Assert.Equal(1, service.GetIdByName("Taxi"));
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData(null)]
        [InlineData("")]
        public void GetIdByNameShouldReturn0(string name)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            Assert.Equal(0, service.GetIdByName(name));
        }

        [Fact]
        public async Task GetByIdShouldReturnTheCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);

            AutoMapperConfig.RegisterMappings(typeof(TestMovieViewModel).Assembly);
            var result = service.GetById<TestMovieViewModel>(1);

            Assert.Equal("Taxi", result.Name);
            Assert.Equal(7.1, result.Rating);
            Assert.Equal(1998, result.Year);
        }

        [Fact]
        public void GetByIdShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            var result = service.GetById<TestMovieViewModel>(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);
            await service.AddAsync(this.movie);

            var result = service.GetAll<TestMovieViewModel>(null);

            Assert.Equal(2, result.Count());
            Assert.Equal("Taxi", result.FirstOrDefault().Name);
        }

        [Fact]
        public async Task GetAllShouldReturnOneModelWithTakeParameterBeeing1()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);
            await service.AddAsync(this.movie);

            var result = service.GetAll<TestMovieViewModel>(1);

            Assert.Single(result);
            Assert.Equal("Taxi", result.FirstOrDefault().Name);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectlyWithAllParametersFilled()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            var diffMovie = new AddMovieInputModel
            {
                Name = "Taxi 2",
                Year = 1998,
                Actors = "Samy Naceri, Frédéric Diefenthal, Marion Cotillard",
                AgeRestriction = 0,
                Description = "funny",
                Director = "Gérard Pirès",
                Duration = 86,
                Rating = 7.1,
                Price = 8,
                Genre = "Action, Comedy, Crime",
                TrailerUrl = "url",
                TrailerVideoUrl = "url",
            };

            await service.AddAsync(this.movie);
            await service.AddAsync(diffMovie);
            await service.AddAsync(this.movie);

            var result = service.GetAll<TestMovieViewModel>(1, 1);

            Assert.Single(result);
            Assert.Equal("Taxi 2", result.FirstOrDefault().Name);
        }

        [Fact]
        public async Task EditMovieShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            var diffMovie = new AddMovieInputModel
            {
                Id = 1,
                Name = "Taxi 2",
                Year = 2000,
                Actors = "Samy Naceri, Frédéric Diefenthal, Marion Cotillard",
                AgeRestriction = 0,
                Description = "funny",
                Director = "Gérard Pirès",
                Duration = 86,
                Rating = 7.5,
                Price = 8,
                Genre = "Action, Comedy, Crime",
                TrailerUrl = "url",
                TrailerVideoUrl = "url",
            };

            await service.AddAsync(this.movie);
            await service.EditAsync(diffMovie);

            Assert.Equal("Taxi 2", repository.All().FirstOrDefault().Name);
            Assert.Equal(2000, repository.All().FirstOrDefault().Year);
            Assert.Equal(7.5, repository.All().FirstOrDefault().Rating);
        }

        [Fact]
        public async Task EditMovieShouldThrowIfIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            var diffMovie = new AddMovieInputModel
            {
                Id = 5,
                Name = "Taxi 2",
                Year = 2000,
                Actors = "Samy Naceri, Frédéric Diefenthal, Marion Cotillard",
                AgeRestriction = 0,
                Description = "funny",
                Director = "Gérard Pirès",
                Duration = 86,
                Rating = 7.5,
                Price = 8,
                Genre = "Action, Comedy, Crime",
                TrailerUrl = "url",
                TrailerVideoUrl = "url",
            };

            await service.AddAsync(this.movie);

            Assert.Throws<ArgumentNullException>(() => service.EditAsync(diffMovie).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task DeleteMovieShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);
            await service.DeleteAsync(1);

            var movie = await repository.GetByIdWithDeletedAsync(1);

            Assert.True(movie.IsDeleted);
        }

        [Fact]
        public async Task DeleteMovieShouldThrowWithIncorrectId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(repository);

            await service.AddAsync(this.movie);

            Assert.Throws<ArgumentNullException>(() => service.DeleteAsync(2).GetAwaiter().GetResult());
        }
    }
}
