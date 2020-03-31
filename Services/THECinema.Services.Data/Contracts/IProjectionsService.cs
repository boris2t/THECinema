namespace THECinema.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Projections;

    public interface IProjectionsService
    {
        Task AddAsync(AddProjectionInputModel inputModel);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetById<T>(int filmId);

        T GetByProjectionId<T>(string id);

        Task EditAsync(AddProjectionInputModel inputModel);

        Task DeleteAsync(string id);
    }
}
