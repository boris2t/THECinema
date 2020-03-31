namespace CinemaSystem.Web.ViewModels.Projections
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class MovieDropDownViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}