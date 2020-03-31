namespace THECinema.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Payments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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

            this.TempData["reservationId"] = inputModel.ReservationId;

            if (inputModel.PaymentType == "Cash")
            {
                return this.RedirectToAction("ShowTicket");
            }

            return this.View();
        }

        public async Task<IActionResult> ShowTicket()
        {
            var reservationId = this.TempData["reservationId"].ToString();
            var viewModel = await this.reservationsService.GetByIdAsync(reservationId);
            return this.View(viewModel);
        }
    }
}
