namespace CinemaSystem.Data.Models
{
    using System.Collections.Generic;

    using CinemaSystem.Data.Common.Models;

    public class Seat : BaseDeletableModel<int>
    {
        public Seat()
        {
            this.Projections = new HashSet<ProjectionSeat>();
        }

        public int HallId { get; set; }

        public virtual Hall Hall { get; set; }

        public IEnumerable<ProjectionSeat> Projections { get; set; }
    }
}
