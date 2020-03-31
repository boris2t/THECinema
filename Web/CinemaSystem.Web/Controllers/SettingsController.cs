namespace CinemaSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CinemaSystem.Data.Common.Repositories;
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Data.Contracts;
    using CinemaSystem.Web.ViewModels.Settings;

    using Microsoft.AspNetCore.Mvc;

    public class SettingsController : BaseController
    {
        private readonly ISettingsService settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

    }
}
