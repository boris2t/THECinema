namespace THECinema.Web.ViewModels.Reservations
{
    using System;

    public class FullInfoReservationViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string MovieName { get; set; }

        public DateTime DateTime { get; set; }

        public string SelectedSeats { get; set; }

        public double Price { get; set; }

        public int HallId { get; set; }

        public string ProjectionType { get; set; }
    }
}
