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
        private readonly DbContextOptionsBuilder<ApplicationDbContext> options;

        public HallsServiceTests()
        {
            this.hall = new AddHallInputModel
            {
                ProjectionType = "TwoD",
                Seats = 50,
            };

            AutoMapperConfig.RegisterMappings(typeof(TestHallViewModel).Assembly);
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task AddHallShouldAddCorrectCount()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddHallShouldAddCorrectData()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            var dbHall = await repository.GetByIdWithDeletedAsync(1);

            Assert.Equal(ProjectionType.TwoD, dbHall.ProjectionType);
            Assert.Equal(50, dbHall.Seats.Count());
        }

        [Fact]
        public async Task DeleteHallShouldWorkCorrectly()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            await service.DeleteAsync(1);

            var dbHall = await repository.GetByIdWithDeletedAsync(1);

            Assert.True(dbHall.IsDeleted);
        }

        [Fact]
        public void DeleteHallShouldThrowIfInvalidId()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            Assert.Throws<ArgumentNullException>(() => service.DeleteAsync(1).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectData()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            await service.AddAsync(this.hall);
            await service.AddAsync(this.hall);

            var result = service.GetAll<TestHallViewModel>();

            Assert.Equal(2, result.Count());
            Assert.Equal(ProjectionType.TwoD, result.FirstOrDefault().ProjectionType);
        }

        [Fact]
        public void GetAllShouldReturnEmptyList()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            var result = service.GetAll<TestHallViewModel>();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectData()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            var diffHall = new AddHallInputModel
            {
                ProjectionType = "FourDx",
                Seats = 100,
            };

            await service.AddAsync(this.hall);
            await service.AddAsync(diffHall);

            var result = service.GetById<TestHallViewModel>(2);

            Assert.Equal(ProjectionType.FourDx, result.ProjectionType);
        }

        [Fact]
        public void GetByIdShouldReturnNullIfInvalidId()
        {
            var repository = new EfDeletableEntityRepository<Hall>(new ApplicationDbContext(this.options.Options));
            var service = new HallsService(repository);

            var result = service.GetById<TestHallViewModel>(2);

            Assert.Null(result);
        }
    }
}
