namespace THECinema.Web.ViewModels.Projections
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class HallDropDownViewModel : IMapFrom<Hall>
    {
        public int Id { get; set; }
    }
}