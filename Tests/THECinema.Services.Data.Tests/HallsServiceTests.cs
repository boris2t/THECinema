namespace THECinema.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using THECinema.Data;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Data.Repositories;
    using THECinema.Services.Data.Tests.TestModels;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Halls;
    using Xunit;

    public class HallsServiceTests
    {
        private readonly AddHallInputModel hall;

        public HallsServiceTests()
        {
            this.hall = new AddHallInputModel
            {
                ProjectionType = "TwoD",
                Seats = 50,
            };
        }

        [Fact]
        public async Task AddHallShouldAddCorrectCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddHallShouldAddCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            var dbHall = await repository.GetByIdWithDeletedAsync(1);

            Assert.Equal(ProjectionType.TwoD, dbHall.ProjectionType);
            Assert.Equal(50, dbHall.Seats.Count());
        }

        [Fact]
        public async Task DeleteHallShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            await service.DeleteAsync(1);

            var dbHall = await repository.GetByIdWithDeletedAsync(1);

            Assert.True(dbHall.IsDeleted);
        }

        [Fact]
        public void DeleteHallShouldThrowIfInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            Assert.Throws<ArgumentNullException>(() => service.DeleteAsync(1).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            await service.AddAsync(this.hall);

            AutoMapperConfig.RegisterMappings(typeof(TestHallViewModel).Assembly);
            var result = service.GetAll<TestHallViewModel>();

            Assert.Equal(2, result.Count());
            Assert.Equal(ProjectionType.TwoD, result.FirstOrDefault().ProjectionType);
        }

        [Fact]
        public void GetAllShouldReturnEmptyList()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(TestHallViewModel).Assembly);
            var result = service.GetAll<TestHallViewModel>();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            var diffHall = new AddHallInputModel
            {
                ProjectionType = "FourDx",
                Seats = 100,
            };

            await service.AddAsync(this.hall);
            await service.AddAsync(diffHall);

            AutoMapperConfig.RegisterMappings(typeof(TestHallViewModel).Assembly);
            var result = service.GetById<TestHallViewModel>(2);

            Assert.Equal(ProjectionType.FourDx, result.ProjectionType);
        }

        [Fact]
        public void GetByIdShouldReturnNullIfInvalidId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(options.Options));
            var service = new HallsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(TestHallViewModel).Assembly);
            var result = service.GetById<TestHallViewModel>(2);

            Assert.Null(result);
        }
    }
}
