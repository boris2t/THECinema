namespace THECinema.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Reservations;

    public interface IReservationsService
    {
        ReservationViewModel GetDetails(string projectionId);

        Task<string> AddAsync(string selectedSeats, double price, string projectionId, string userId);

        Task<FullInfoReservationViewModel> GetByIdAsync(string reservationId);

        Task<IEnumerable<FullInfoReservationViewModel>> GetReservationsByUserIdAsync(string userId);

        Task DeleteAsync(string id);

        IEnumerable<string> GetSeatIds(string reservationId);

        Task MakeSeatsTakenAsync(IEnumerable<string> seatIds, string reservationId);

        Task MakeSeatsFreeAsync(IEnumerable<string> seatIds);

        ParseReservationDataModel ParseData(string priceInput, string seatsInput);

        string GenerateEmailContent(FullInfoReservationViewModel viewModel);
    }
}
