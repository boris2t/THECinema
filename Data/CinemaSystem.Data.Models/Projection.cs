namespace CinemaSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaSystem.Data.Common.Models;

    public class Projection : BaseDeletableModel<string>
    {
        public Projection()
        {
            this.Seats = new HashSet<ProjectionSeat>();
        }

        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int HallId { get; set; }

        public virtual Hall Hall { get; set; }

        public DateTime ProjectionDateTime { get; set; }

        public IEnumerable<ProjectionSeat> Seats { get; set; }
    }
}
