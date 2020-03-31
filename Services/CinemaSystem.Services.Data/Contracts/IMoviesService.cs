namespace CinemaSystem.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaSystem.Web.ViewModels.Movies;

    public interface IMoviesService
    {
        Task AddAsync(AddMovieInputModel inputModel);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(int filmId);

        Task DeleteAsync(int filmId);

        Task EditAsync(AddMovieInputModel inputModel);

        int GetIdByName(string filmName);
    }
}
