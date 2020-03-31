namespace CinemaSystem.Web.ViewModels.Projections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaSystem.Data.Models;
    using CinemaSystem.Services.Mapping;

    public class AddProjectionInputModel : IMapTo<Projection>, IMapFrom<Projection>
    {
        public string Id { get; set; }

        [Display(Name = "Hall")]
        public int HallId { get; set; }

        [Display(Name = "Movie")]
        public int MovieId { get; set; }

        [Display(Name = "Projection Date and Time")]
        public DateTime ProjectionDateTime { get; set; }

        public IEnumerable<MovieDropDownViewModel> Movies { get; set; }

        public IEnumerable<HallDropDownViewModel> Halls { get; set; }
    }
}
