namespace THECinema.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Reviews;

    public interface IReviewsService
    {
        Task<ReviewViewModel> AddAsync(AddReviewInputModel inputModel);

        IEnumerable<T> GetAllByMovieId<T>(int movieId);

        Task DeleteAsync(int id);

        Task<ReviewViewModel> EditAsync(AddReviewInputModel inputModel);
    }
}
