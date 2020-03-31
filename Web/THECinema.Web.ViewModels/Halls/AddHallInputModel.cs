namespace THECinema.Web.ViewModels.Halls
{
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class AddHallInputModel : IMapTo<Hall>
    {
        [Required]
        [Display(Name = "Projection Type")]
        public string ProjectionType { get; set; }

        [Range(50, 100)]
        public int Seats { get; set; }
    }
}
