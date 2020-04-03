namespace THECinema.Web.ViewModels.Reviews
{
    using System;
    using System.Collections.Generic;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Comments;

    public class ReviewViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ApplicationUserUserName { get; set; }

        public double Stars { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
