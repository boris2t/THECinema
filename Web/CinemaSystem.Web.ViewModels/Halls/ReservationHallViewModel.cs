namespace CinemaSystem.Web.ViewModels.Halls
{
    using CinemaSystem.Data.Models;
    using CinemaSystem.Data.Models.Enums;
    using CinemaSystem.Services.Mapping;

    public class ReservationHallViewModel : IMapFrom<Hall>, IMapTo<Hall>
    {
        public int Id { get; set; }

        public ProjectionType ProjectionType { get; set; }
    }
}
