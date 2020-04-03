namespace THECinema.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Payments;
    using THECinema.Web.ViewModels.Reservations;

    [Authorize]
    public class PaymentsController : BaseController
    {
        private readonly IReservationsService reservationsService;
        private readonly IPaymentsService paymentsService;

        public PaymentsController(
            IReservationsService reservationsService,
            IPaymentsService paymentsService)
        {
            this.reservationsService = reservationsService;
            this.paymentsService = paymentsService;
        }

        public async Task<IActionResult> Checkout(PaymentTypeInputModel inputModel)
        {
            var seatIds = this.TempData["seatIds"] as IEnumerable<string>;

            await this.reservationsService.MakeSeatsTakenAsync(seatIds, inputModel.ReservationId);
            await this.paymentsService.AddAsync(inputModel);

            if (inputModel.PaymentType == "Cash")
            {
                return this.RedirectToAction("ShowTicket", new { reservationId = inputModel.ReservationId });
            }

            var viewModel = new SimpleReservationViewModel
            {
                ReservationId = inputModel.ReservationId,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> ShowTicket(string reservationId)
        {
            var viewModel = await this.reservationsService.GetByIdAsync(reservationId);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}
