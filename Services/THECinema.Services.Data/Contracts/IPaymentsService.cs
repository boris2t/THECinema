namespace THECinema.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using THECinema.Web.ViewModels.Payments;

    public interface IPaymentsService
    {
        Task AddAsync(PaymentTypeInputModel inputModel);
    }
}
