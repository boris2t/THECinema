namespace THECinema.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Halls;
    using Microsoft.AspNetCore.Mvc;

    public class HallsController : AdministrationController
    {
        private readonly IHallsService hallsService;

        public HallsController(IHallsService hallsService)
        {
            this.hallsService = hallsService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddHallInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.hallsService.AddAsync(inputModel);

            return this.Redirect("/");
        }
    }
}
