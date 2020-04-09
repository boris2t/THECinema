namespace THECinema.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using THECinema.Data;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Data.Repositories;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Halls;
    using THECinema.Web.ViewModels.Movies;
    using THECinema.Web.ViewModels.Projections;
    using Xunit;

    public class ReservationsServiceTests
    {
        private readonly Reservation reservation;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> options;

        public ReservationsServiceTests()
        {
            this.reservation = new Reservation
            {
                SelectedSeats = "A1 B1",
                Price = 5,
                ProjectionId = "projectionId",
                ApplicationUserId = "f00",
            };

            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task AddShouldAddCorrectCount()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            await service.AddAsync(this.reservation.SelectedSeats, this.reservation.Price, this.reservation.ProjectionId, this.reservation.ApplicationUserId);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddShouldAddCorrectData()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            await service.AddAsync(
                this.reservation.SelectedSeats,
                this.reservation.Price,
                this.reservation.ProjectionId,
                this.reservation.ApplicationUserId);

            var dbReservation = repository.All().FirstOrDefault();
            Assert.Equal("A1 B1", dbReservation.SelectedSeats);
        }

        [Fact]
        public async Task DeleteShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            await service.AddAsync(
                this.reservation.SelectedSeats,
                this.reservation.Price,
                this.reservation.ProjectionId,
                this.reservation.ApplicationUserId);

            var id = repository.All().Select(r => r.Id).FirstOrDefault();
            await service.DeleteAsync(id);

            var dbReservation = await repository.GetByIdWithDeletedAsync(id);
            Assert.True(dbReservation.IsDeleted);
        }

        [Fact]
        public void DeleteShouldThrowIfInvalidId()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            Assert.Throws<ArgumentNullException>(() => service.DeleteAsync("Invalid id").GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GetByIdShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            await service.AddAsync(
                this.reservation.SelectedSeats,
                10,
                this.reservation.ProjectionId,
                this.reservation.ApplicationUserId);

            await service.AddAsync(
                this.reservation.SelectedSeats,
                this.reservation.Price,
                this.reservation.ProjectionId,
                this.reservation.ApplicationUserId);

            var id = repository.All().Select(r => r.Id).FirstOrDefault();
            var result = await service.GetByIdAsync(id);

            Assert.Equal(10, result.Price);
        }

        [Fact]
        public async Task GetByIdShouldReturnNullIfInvalidId()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            var result = await service.GetByIdAsync("Invalid");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetDetailsShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            await service.AddAsync(
                this.reservation.SelectedSeats,
                this.reservation.Price,
                this.reservation.ProjectionId,
                this.reservation.ApplicationUserId);

            var id = repository.All().Select(r => r.Id).FirstOrDefault();
            var result = service.GetDetails("projectionId");

            Assert.Equal(12, result.Movie.Price);
            Assert.Equal("Taxi 2", result.Movie.Name);
            Assert.Equal(ProjectionType.TwoD, result.Hall.ProjectionType);
        }

        [Fact]
        public void GetDetailsShouldReturnNullIfInvalidId()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            var result = service.GetDetails("Invalid");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetReservationByUserIdShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            await service.AddAsync(
                this.reservation.SelectedSeats,
                this.reservation.Price,
                this.reservation.ProjectionId,
                this.reservation.ApplicationUserId);

            var result = await service.GetReservationsByUserIdAsync("f00");
            var resultReservation = result.FirstOrDefault();

            Assert.Equal(5, resultReservation.Price);
        }

        [Fact]
        public async Task GetReservationByUserIdShouldReturnEmptyListIfInvalidId()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            await service.AddAsync(
                this.reservation.SelectedSeats,
                this.reservation.Price,
                this.reservation.ProjectionId,
                this.reservation.ApplicationUserId);

            var result = await service.GetReservationsByUserIdAsync("f000");

            Assert.Empty(result);
        }

        [Fact]
        public void GetSeatIdsShouldWorkCorrectly()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            var result = service.GetSeatIds("reservationId");

            Assert.Equal("firstSeatId", result.ToArray()[0]);
        }

        [Fact]
        public void GetSeatIdsShouldReturnEmptyListIfReservationIdIsInvalid()
        {
            var context = new ApplicationDbContext(this.options.Options);
            var repository = new EfDeletableEntityRepository<Reservation>(context);
            var service = this.GetReservationsService(repository, context);

            var result = service.GetSeatIds("Invalid");

            Assert.Empty(result);
        }

        private ReservationsService GetReservationsService(
           EfDeletableEntityRepository<Reservation> reservationsRepository,
           ApplicationDbContext context)
        {
            var projectionsServiceMock = new Mock<IProjectionsService>();
            projectionsServiceMock.Setup(ps => ps.GetByProjectionId<ProjectionViewModel>("projectionId"))
                .Returns(new ProjectionViewModel
                {
                    MovieId = 1,
                    HallId = 1,
                    ProjectionDateTime = new DateTime(2020, 04, 09),
                });

            var projectionSeatRepositoryMock = new Mock<EfDeletableEntityRepository<ProjectionSeat>>(context);
            projectionSeatRepositoryMock.Setup(ps => ps.All())
                .Returns(new List<ProjectionSeat>
                {
                    new ProjectionSeat { Id = "firstSeatId", ReservationId = "reservationId" },
                    new ProjectionSeat { Id = "secondSeatId", ReservationId = "reservationId" },
                }.AsQueryable());

            var hallsServiceMock = new Mock<IHallsService>();
            hallsServiceMock.Setup(hs => hs.GetById<ReservationHallViewModel>(1))
                .Returns(new ReservationHallViewModel
                {
                    Id = 1,
                    ProjectionType = ProjectionType.TwoD,
                });

            var moviesServiceMock = new Mock<IMoviesService>();
            moviesServiceMock.Setup(ms => ms.GetById<FullInfoMovieViewModel>(1))
                .Returns(new FullInfoMovieViewModel
                {
                    Name = "Taxi",
                });

            moviesServiceMock.Setup(ms => ms.GetById<ReservationMovieViewModel>(1))
               .Returns(new ReservationMovieViewModel
               {
                   Price = 12,
                   Name = "Taxi 2",
               });

            var userStoreMock = Mock.Of<IUserStore<ApplicationUser>>();
            var userMgr = new Mock<UserManager<ApplicationUser>>(userStoreMock, null, null, null, null, null, null, null, null);
            var user = new ApplicationUser() { Id = "f00", UserName = "f00", Email = "f00@example.com" };
            var tcs = new TaskCompletionSource<ApplicationUser>();
            tcs.SetResult(user);
            userMgr.Setup(x => x.FindByIdAsync("f00")).Returns(tcs.Task);

            var projectionsService = new ReservationsService(
                reservationsRepository,
                projectionsServiceMock.Object,
                hallsServiceMock.Object,
                moviesServiceMock.Object,
                projectionSeatRepositoryMock.Object,
                userMgr.Object);

            return projectionsService;
        }
    }
}
