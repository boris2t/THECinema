namespace CinemaSystem.Web.ViewModels.Halls
{
    using System.ComponentModel.DataAnnotations;

    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class AddHallInputModel : IMapTo<Hall>
    {
        [Required]
        [Display(Name = "Projection Type")]
        public string ProjectionType { get; set; }

        [Range(50, 100)]
        public int Seats { get; set; }
    }
}
