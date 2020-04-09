namespace THECinema.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using THECinema.Data;
    using THECinema.Data.Models;
    using THECinema.Data.Repositories;
    using THECinema.Services.Data.Tests.TestModels;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Projections;
    using Xunit;

    public class ProjectionsServiceTests
    {
        private readonly AddProjectionInputModel projection;

        public ProjectionsServiceTests()
        {
            this.projection = new AddProjectionInputModel
            {
                HallId = 1,
                MovieId = 1,
                ProjectionDateTime = new DateTime(2008, 4, 10),
            };
            AutoMapperConfig.RegisterMappings(typeof(TestProjectionViewModel).Assembly);
        }

        [Fact]
        public async Task AddProjectionShouldAddCorrectCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            await service.AddAsync(this.projection);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddProjectionShouldAddCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            await service.AddAsync(this.projection);
            var dbProjection = repository.All().FirstOrDefault();

            Assert.Equal("9.4.2008 г.", dbProjection.ProjectionDateTime.ToShortDateString());
            Assert.Equal(1, dbProjection.HallId);
        }

        [Fact]
        public async Task DeleteProjectionShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            await service.AddAsync(this.projection);
            var id = repository.All().Select(p => p.Id).FirstOrDefault();
            await service.DeleteAsync(id);

            var dbProjection = await repository.GetByIdWithDeletedAsync(id);
            Assert.True(dbProjection.IsDeleted);
        }

        [Fact]
        public void DeleteProjectionShouldThrowIfInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            Assert.Throws<ArgumentNullException>(() => service.DeleteAsync("invalid id").GetAwaiter().GetResult());
        }

        [Fact]
        public async Task EditeProjectionShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            await service.AddAsync(this.projection);
            var id = repository.All().Select(p => p.Id).FirstOrDefault();

            var diffProjection = new AddProjectionInputModel
            {
                Id = id,
                HallId = 2,
                MovieId = 3,
                ProjectionDateTime = new DateTime(2020, 05, 10),
            };

            await service.EditAsync(diffProjection);
            var dbProjection = await repository.GetByIdWithDeletedAsync(id);

            Assert.Equal(2, dbProjection.HallId);
            Assert.Equal(3, dbProjection.MovieId);
        }

        [Fact]
        public void EditProjectionShouldThrowIfInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            var diffProjection = new AddProjectionInputModel
            {
                Id = "Invalid",
                HallId = 2,
                MovieId = 3,
                ProjectionDateTime = new DateTime(2020, 05, 10),
            };

            Assert.Throws<ArgumentNullException>(() => service.EditAsync(diffProjection).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            await service.AddAsync(this.projection);
            await service.AddAsync(this.projection);

            var result = service.GetAll<TestProjectionViewModel>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            await service.AddAsync(this.projection);
            await service.AddAsync(this.projection);

            var result = service.GetAll<TestProjectionViewModel>();

            var dbProjection = result.FirstOrDefault();
            Assert.Equal(1, dbProjection.MovieId);
        }

        [Fact]
        public async Task GetByIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            var diffProjection = new AddProjectionInputModel
            {
                HallId = 2,
                MovieId = 3,
                ProjectionDateTime = new DateTime(2020, 05, 10),
            };

            await service.AddAsync(diffProjection);
            await service.AddAsync(this.projection);

            var result = service.GetById<TestProjectionViewModel>(3);

            var dbProjection = result.FirstOrDefault();
            Assert.Equal(2, dbProjection.HallId);
        }

        [Fact]
        public void GetByIdShouldReturnNullIfInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            var result = service.GetById<TestProjectionViewModel>(5);

            var dbProjection = result.FirstOrDefault();
            Assert.Null(dbProjection);
        }

        [Fact]
        public async Task GetByProjectionIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            var diffProjection = new AddProjectionInputModel
            {
                HallId = 2,
                MovieId = 3,
                ProjectionDateTime = new DateTime(2020, 05, 10),
            };

            await service.AddAsync(diffProjection);
            await service.AddAsync(this.projection);
            var id = repository.All().Select(p => p.Id).FirstOrDefault();

            var result = service.GetByProjectionId<TestProjectionViewModel>(id);

            Assert.Equal(2, result.HallId);
            Assert.Equal(3, result.MovieId);
        }

        [Fact]
        public void GetByProjectionIdShouldReturnNullIfInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ApplicationDbContext(options.Options);
            var repository = new EfDeletableEntityRepository<Projection>(context);
            var service = this.GetProjectionsService(repository, context);

            var result = service.GetByProjectionId<TestProjectionViewModel>("Invalid");

            Assert.Null(result);
        }

        private ProjectionsService GetProjectionsService(
            EfDeletableEntityRepository<Projection> projectionsRepository,
            ApplicationDbContext context)
        {
            var seatsRepositoryMock = new Mock<EfDeletableEntityRepository<Seat>>(context);
            var projectionSeatRepositoryMock = new Mock<EfDeletableEntityRepository<ProjectionSeat>>(context);

            var listOfSeats = new List<Seat>();
            for (int i = 0; i < 50; i++)
            {
                listOfSeats.Add(new Seat { HallId = 1 });
            }

            seatsRepositoryMock.Setup(s => s.All()).Returns(listOfSeats.AsQueryable);

            var projectionsService = new ProjectionsService(
                projectionsRepository,
                projectionSeatRepositoryMock.Object,
                seatsRepositoryMock.Object);

            return projectionsService;
        }
    }
}
