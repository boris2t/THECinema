namespace THECinema.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Payments;
    using THECinema.Web.ViewModels.Reservations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReservationsController : BaseController
    {
        private readonly IReservationsService reservationsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservationsController(
            IReservationsService reservationsService,
            UserManager<ApplicationUser> userManager)
        {
            this.reservationsService = reservationsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(string projectionId)
        {
            var viewModel = new CombinedReservationViewModel
            {
                ViewModel = await this.reservationsService.GetDetailsAsync(projectionId),
                InputModel = new ReservationInputModel(),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(ReservationInputModel inputModel)
        {
            var model = this.reservationsService.ParseData(inputModel.Price, inputModel.SelectedSeats);
            var userId = this.userManager.GetUserId(this.User);

            var reservationId = await this.reservationsService.AddAsync(model.SelectedSeats, model.Price, inputModel.ProjectionId, userId);

            this.TempData["seatIds"] = model.SelectedSeatsIds.ToList();

            return this.RedirectToAction("GetById", new { reservationId });
        }

        public async Task<IActionResult> GetById(string reservationId)
        {
            var viewModel = new CombinedPaymentModel
            {
                ViewModel = await this.reservationsService.GetByIdAsync(reservationId),
                InputModel = new PaymentTypeInputModel
                {
                    ReservationId = reservationId,
                },
            };

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.reservationsService.DeleteAsync(id);
            var seatIds = this.reservationsService.GetSeatIds(id);
            await this.reservationsService.MakeSeatsFreeAsync(seatIds);

            return this.Redirect("/Identity/Account/Manage");
        }
    }
}
