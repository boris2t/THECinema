namespace CinemaSystem.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaSystem.Web.ViewModels.Halls;

    public interface IHallsService
    {
        Task AddAsync(AddHallInputModel inputModel);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(int id);

        Task DeleteAsync(int id);
    }
}
