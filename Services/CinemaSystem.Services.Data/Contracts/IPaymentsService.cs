namespace CinemaSystem.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using CinemaSystem.Web.ViewModels.Payments;

    public interface IPaymentsService
    {
        Task AddAsync(PaymentTypeInputModel inputModel);
    }
}
