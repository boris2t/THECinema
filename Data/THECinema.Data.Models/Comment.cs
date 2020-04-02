namespace THECinema.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using THECinema.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        [Required]
        public string Content { get; set; }

        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
