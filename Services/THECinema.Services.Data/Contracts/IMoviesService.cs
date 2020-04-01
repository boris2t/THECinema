namespace THECinema.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Movies;

    public interface IMoviesService
    {
        Task AddAsync(AddMovieInputModel inputModel);

        IEnumerable<T> GetAll<T>(int? take = 0, int skip = 0);

        T GetById<T>(int filmId);

        Task DeleteAsync(int filmId);

        Task EditAsync(AddMovieInputModel inputModel);

        int GetIdByName(string filmName);

        int GetMoviesCount();
    }
}
