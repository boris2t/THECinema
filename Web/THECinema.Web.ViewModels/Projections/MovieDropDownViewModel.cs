namespace THECinema.Web.ViewModels.Projections
{
    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class MovieDropDownViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}