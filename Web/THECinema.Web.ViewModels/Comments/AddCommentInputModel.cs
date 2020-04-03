namespace THECinema.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Models;
    using THECinema.Services.Mapping;

    public class AddCommentInputModel : IMapTo<Comment>
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int ReviewId { get; set; }

        public int MovieId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}
