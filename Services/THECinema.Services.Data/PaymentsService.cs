namespace THECinema.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Services.Data.Contracts;
    using THECinema.Web.ViewModels.Payments;

    public class PaymentsService : IPaymentsService
    {
        private readonly IDeletableEntityRepository<Payment> paymentsRepository;

        public PaymentsService(IDeletableEntityRepository<Payment> paymentsRepository)
        {
            this.paymentsRepository = paymentsRepository;
        }

        public async Task AddAsync(PaymentTypeInputModel inputModel)
        {
            var paymentType = Enum.Parse(typeof(PaymentType), inputModel.PaymentType);

            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                PaymentType = (PaymentType)paymentType,
                ReservationId = inputModel.ReservationId,
            };

            await this.paymentsRepository.AddAsync(payment);
            await this.paymentsRepository.SaveChangesAsync();
        }
    }
}
