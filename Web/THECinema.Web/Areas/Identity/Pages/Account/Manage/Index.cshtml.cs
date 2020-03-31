namespace THECinema.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Reservations;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IReservationsService reservationsService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IReservationsService reservationsService)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.reservationsService = reservationsService;

            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var reservations = this.reservationsService.GetReservationsByUserIdAsync(userId).GetAwaiter().GetResult();

            this.Reservations = reservations;
        }

        public IEnumerable<FullInfoReservationViewModel> Reservations { get; set; }
    }
}
