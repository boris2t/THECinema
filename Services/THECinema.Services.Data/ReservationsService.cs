namespace THECinema.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Halls;
    using THECinema.Web.ViewModels.Movies;
    using THECinema.Web.ViewModels.Reservations;
    using Microsoft.AspNetCore.Identity;

    public class ReservationsService : IReservationsService
    {
        private readonly IDeletableEntityRepository<Projection> projectionsRepository;
        private readonly IHallsService hallsService;
        private readonly IMoviesService moviesService;
        private readonly IDeletableEntityRepository<ProjectionSeat> seatsRepository;
        private readonly IDeletableEntityRepository<Reservation> reservationsRepository;
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IRepository<Hall> hallRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservationsService(
            IDeletableEntityRepository<Projection> projectionsRepository,
            IHallsService hallsService,
            IMoviesService moviesService,
            IDeletableEntityRepository<ProjectionSeat> seatsRepository,
            IDeletableEntityRepository<Reservation> reservationsRepository,
            IDeletableEntityRepository<Movie> moviesRepository,
            IRepository<Hall> hallRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.projectionsRepository = projectionsRepository;
            this.hallsService = hallsService;
            this.moviesService = moviesService;
            this.seatsRepository = seatsRepository;
            this.reservationsRepository = reservationsRepository;
            this.moviesRepository = moviesRepository;
            this.hallRepository = hallRepository;
            this.userManager = userManager;
        }

        public async Task<string> AddAsync(string selectedSeats, double price, string projectionId, string userId)
        {
            var reservation = new Reservation
            {
                Id = Guid.NewGuid().ToString(),
                ProjectionId = projectionId,
                SelectedSeats = selectedSeats,
                ApplicationUserId = userId,
                Price = price,
            };

            await this.reservationsRepository.AddAsync(reservation);
            await this.reservationsRepository.SaveChangesAsync();

            return reservation.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var reservation = this.reservationsRepository.All().Where(r => r.Id == id).FirstOrDefault();
            this.reservationsRepository.Delete(reservation);
            await this.reservationsRepository.SaveChangesAsync();
        }

        public async Task<FullInfoReservationViewModel> GetByIdAsync(string reservationId)
        {
            var reservation = await this.reservationsRepository.GetByIdWithDeletedAsync(reservationId);
            var projection = await this.projectionsRepository.GetByIdWithDeletedAsync(reservation.ProjectionId);
            var movie = await this.moviesRepository.GetByIdWithDeletedAsync(projection.MovieId);
            var hall = this.hallRepository.All().Where(h => h.Id == projection.HallId).FirstOrDefault();
            var user = await this.userManager.FindByIdAsync(reservation.ApplicationUserId);

            var viewModel = new FullInfoReservationViewModel
            {
                Id = reservationId,
                Price = reservation.Price,
                SelectedSeats = reservation.SelectedSeats,
                MovieName = movie.Name,
                HallId = projection.HallId,
                ProjectionType = Enum.GetName(typeof(ProjectionType), hall.ProjectionType),
                UserName = user.Name,
                DateTime = projection.ProjectionDateTime,
            };

            return viewModel;
        }

        public async Task<ReservationViewModel> GetDetailsAsync(string projectionId)
       {
            var projection = await this.projectionsRepository.GetByIdWithDeletedAsync(projectionId);
            var movie = this.moviesService.GetById<ReservationMovieViewModel>(projection.MovieId);
            var hall = this.hallsService.GetById<ReservationHallViewModel>(projection.HallId);
            var seats = new Dictionary<string, List<ProjectionSeat>>();

            var allSeats = this.seatsRepository.All().Where(s => s.ProjectionId == projectionId).ToList();

            if (allSeats.Count() == 50)
            {
                seats["firstRow"] = allSeats.GetRange(0, 10);
                seats["secondRow"] = allSeats.GetRange(10, 10);
                seats["thirdRow"] = allSeats.GetRange(20, 10);
                seats["fourthRow"] = allSeats.GetRange(30, 10);
                seats["fifthRow"] = allSeats.GetRange(40, 10);
            }
            else if (allSeats.Count() == 100)
            {
                seats["firstRow"] = allSeats.GetRange(0, 20);
                seats["secondRow"] = allSeats.GetRange(20, 20);
                seats["thirdRow"] = allSeats.GetRange(40, 20);
                seats["fourthRow"] = allSeats.GetRange(60, 20);
                seats["fifthRow"] = allSeats.GetRange(80, 20);
            }

            var viewModel = new ReservationViewModel
            {
                Movie = movie,
                Hall = hall,
                Seats = seats,
                Price = movie.Price,
                ProjectionId = projectionId,
            };

            return viewModel;
        }

        public async Task<IEnumerable<FullInfoReservationViewModel>> GetReservationsByUserIdAsync(string userId)
        {
            var reservationIds = this.reservationsRepository
                .All()
                .Where(r => r.ApplicationUserId == userId)
                .Select(r => r.Id)
                .ToList();

            var reservations = new List<FullInfoReservationViewModel>();

            foreach (var id in reservationIds)
            {
                var reservation = await this.GetByIdAsync(id);
                reservations.Add(reservation);
            }

            return reservations;
        }

        public IEnumerable<string> GetSeatIds(string reservationId)
        {
            return this.seatsRepository
                .All()
                .Where(s => s.ReservationId == reservationId)
                .Select(s => s.Id)
                .ToList();
        }

        public async Task MakeSeatsFreeAsync(IEnumerable<string> seatIds)
        {
            foreach (var id in seatIds)
            {
                var seat = this.seatsRepository.All().Where(s => s.Id == id).FirstOrDefault();
                seat.IsTaken = false;
                seat.ReservationId = null;
            }

            await this.seatsRepository.SaveChangesAsync();
        }

        public async Task MakeSeatsTakenAsync(IEnumerable<string> seatIds, string reservationId)
        {
            foreach (var id in seatIds)
            {
                var seat = this.seatsRepository.All().Where(s => s.Id == id).FirstOrDefault();
                seat.IsTaken = true;
                seat.ReservationId = reservationId;
            }

            await this.seatsRepository.SaveChangesAsync();
        }

        public ParseReservationDataModel ParseData(string priceInput, string seatsInput)
        {
            var priceAsString = priceInput;
            priceAsString = priceAsString.Substring(1, priceAsString.Length - 1);
            var price = double.Parse(priceAsString);

            var selectedSeatsArray = seatsInput.Split(",");

            var selectedSeats = string.Empty;
            var selectedSeatsIds = new List<string>();

            foreach (var selectedSeat in selectedSeatsArray)
            {
                var split = selectedSeat.Split("--");

                selectedSeats += split[0] + " ";
                selectedSeatsIds.Add(split[1]);
            }

            var model = new ParseReservationDataModel
            {
                Price = price,
                SelectedSeats = selectedSeats,
                SelectedSeatsIds = selectedSeatsIds,
            };

            return model;
        }
    }
}
