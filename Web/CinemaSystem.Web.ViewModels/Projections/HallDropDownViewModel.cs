namespace CinemaSystem.Web.ViewModels.Projections
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class HallDropDownViewModel : IMapFrom<Hall>
    {
        public int Id { get; set; }
    }
}