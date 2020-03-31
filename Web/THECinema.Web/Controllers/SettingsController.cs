namespace THECinema.Web.Controllers
{
    using THECinema.Services.Data.Contracts;

    public class SettingsController : BaseController
    {
        private readonly ISettingsService settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

    }
}
