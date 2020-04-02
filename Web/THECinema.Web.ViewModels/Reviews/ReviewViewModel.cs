namespace THECinema.Web.ViewModels.Reviews
{
    using System;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class ReviewViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ApplicationUserUserName { get; set; }

        public double Stars { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
