namespace THECinema.Data.Models
{
    using System.Collections.Generic;

    using THECinema.Data.Common.Models;
    using THECinema.Data.Models.Enums;

    public class Hall : BaseDeletableModel<int>
    {
        public Hall()
        {
            this.Movies = new HashSet<Projection>();
            this.Seats = new HashSet<Seat>();
        }

        public ProjectionType ProjectionType { get; set; }

        public IEnumerable<Seat> Seats { get; set; }

        public IEnumerable<Projection> Movies { get; set; }
    }
}
