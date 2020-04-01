namespace THECinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Common.Models;

    public class Movie : BaseDeletableModel<int>
    {
        public Movie()
        {
            this.Halls = new HashSet<Projection>();
        }

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

        public string TrailerUrl { get; set; }

        public string TrailerVideoUrl { get; set; }

        [Required]
        [MaxLength(300)]
        public string Actors { get; set; }

        [Range(0, 18)]
        public int AgeRestriction { get; set; }

        public int Duration { get; set; }

        public double Price { get; set; }

        public IEnumerable<Projection> Halls { get; set; }

        public string Genre { get; set; }
    }
}
