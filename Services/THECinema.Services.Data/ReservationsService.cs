namespace THECinema.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using THECinema.Common;
    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Halls;
    using THECinema.Web.ViewModels.Movies;
    using THECinema.Web.ViewModels.Projections;
    using THECinema.Web.ViewModels.Reservations;

    public class ReservationsService : IReservationsService
    {
        private const string InvalidIdExceptionMessage = "Reservation doesn't exist!";

        private readonly IDeletableEntityRepository<Reservation> reservationsRepository;
        private readonly IProjectionsService projectionsService;
        private readonly IHallsService hallsService;
        private readonly IMoviesService moviesService;
        private readonly IDeletableEntityRepository<ProjectionSeat> seatsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservationsService(
            IDeletableEntityRepository<Reservation> reservationsRepository,
            IProjectionsService projectionsService,
            IHallsService hallsService,
            IMoviesService moviesService,
            IDeletableEntityRepository<ProjectionSeat> seatsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.reservationsRepository = reservationsRepository;
            this.projectionsService = projectionsService;
            this.hallsService = hallsService;
            this.moviesService = moviesService;
            this.seatsRepository = seatsRepository;
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

            if (reservation == null)
            {
                throw new ArgumentNullException(InvalidIdExceptionMessage);
            }

            this.reservationsRepository.Delete(reservation);
            await this.reservationsRepository.SaveChangesAsync();
        }

        public string GenerateEmailContent(FullInfoReservationViewModel viewModel)
        {
            return @$"<h3>A new reservation has been made!</h3>
                        <h5>Name: {viewModel.UserName}</h5>
                        <h5>Movie: {viewModel.MovieName}</h5>
                        <h5>Time: {viewModel.DateTime}</h5>
                        <h5>Seats: {viewModel.SelectedSeats}</h5>
                        <h5>Hall number: {viewModel.HallId}</h5>
                        <h5>Projection Type: {viewModel.ProjectionType}</h5>
                        <h5>Price: ${viewModel.Price.ToString("f2")}</h5>
                        <h4>Thank you for making a reservation at {GlobalConstants.SystemName}. We hope you enjoy the movie and see you soon!</h4>";
        }

        public async Task<FullInfoReservationViewModel> GetByIdAsync(string reservationId)
        {
            var reservation = await this.reservationsRepository.GetByIdWithDeletedAsync(reservationId);

            if (reservation == null)
            {
                return null;
            }

            var projection = this.projectionsService.GetByProjectionId<ProjectionViewModel>(reservation.ProjectionId);
            var movie = this.moviesService.GetById<FullInfoMovieViewModel>(projection.MovieId);
            var hall = this.hallsService.GetById<ReservationHallViewModel>(projection.HallId);
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

        public ReservationViewModel GetDetails(string projectionId)
       {
            var projection = this.projectionsService.GetByProjectionId<ProjectionViewModel>(projectionId);

            if (projection == null)
            {
                return null;
            }

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
            priceAsString = priceAsString[1..];
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
