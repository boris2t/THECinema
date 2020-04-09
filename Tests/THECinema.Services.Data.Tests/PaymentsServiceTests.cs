namespace THECinema.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using THECinema.Data;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Data.Repositories;
    using THECinema.Web.ViewModels.Payments;
    using Xunit;

    public class PaymentsServiceTests
    {
        [Fact]
        public async Task AddPaymentShouldAddCorrectCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Payment>(new ApplicationDbContext(options.Options));
            var service = new PaymentsService(repository);

            var payment = new PaymentTypeInputModel
            {
                PaymentType = "Cash",
                ReservationId = Guid.NewGuid().ToString(),
            };

            await service.AddAsync(payment);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddPaymentShouldAddCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Payment>(new ApplicationDbContext(options.Options));
            var service = new PaymentsService(repository);

            var payment = new PaymentTypeInputModel
            {
                PaymentType = "Cash",
                ReservationId = Guid.NewGuid().ToString(),
            };

            await service.AddAsync(payment);
            var dbPayment = repository.All().FirstOrDefault();

            Assert.Equal(PaymentType.Cash, dbPayment.PaymentType);
        }
    }
}
