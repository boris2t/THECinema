namespace THECinema.Web.ViewModels.Movies
{
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class AddMovieInputModel : IMapTo<Movie>, IMapFrom<Movie>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Range(1900, 2030)]
        public int Year { get; set; }

        [Range(0, 10)]
        public double Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Director { get; set; }

        [Display(Name="Trailer Url")]
        public string TrailerUrl { get; set; }

        [Required]
        [MaxLength(300)]
        public string Actors { get; set; }

        [Display(Name = "Age Restriction")]
        [Range(0, 18)]
        public int AgeRestriction { get; set; }

        [Range(15, 420)]
        public int Duration { get; set; }

        public double Price { get; set; }

        public string Genre { get; set; }
    }
}
