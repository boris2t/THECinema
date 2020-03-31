namespace THECinema.Web.ViewModels.Halls
{
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Services.Mapping;

    public class ReservationHallViewModel : IMapFrom<Hall>, IMapTo<Hall>
    {
        public int Id { get; set; }

        public ProjectionType ProjectionType { get; set; }
    }
}
