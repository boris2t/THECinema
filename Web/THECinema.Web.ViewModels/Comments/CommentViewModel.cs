namespace THECinema.Web.ViewModels.Comments
{
    using System;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class CommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string ApplicationUserUserName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
