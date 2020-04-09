namespace THECinema.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Data.Models.Enums;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Halls;

    public class HallsService : IHallsService
    {
        private readonly IDeletableEntityRepository<Hall> hallsRepository;

        public HallsService(IDeletableEntityRepository<Hall> hallsRepository)
        {
            this.hallsRepository = hallsRepository;
        }

        public async Task AddAsync(AddHallInputModel inputModel)
        {
            var projectionType = Enum.Parse(typeof(ProjectionType), inputModel.ProjectionType);
            var seats = new List<Seat>();

            var hall = new Hall
            {
                ProjectionType = (ProjectionType)projectionType,
                Seats = seats,
            };

            for (int i = 0; i < inputModel.Seats; i++)
            {
                var seat = new Seat
                {
                    HallId = hall.Id,
                };

                seats.Add(seat);
            }

            await this.hallsRepository.AddAsync(hall);
            await this.hallsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hall = this.hallsRepository.All().Where(h => h.Id == id).FirstOrDefault();

            if (hall == null)
            {
                throw new ArgumentNullException("The Hall doesn't exist!");
            }

            this.hallsRepository.Delete(hall);
            await this.hallsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.hallsRepository
                .All()
                .To<T>()
                .ToList();
        }

        public T GetById<T>(int id)
        {
            return this.hallsRepository
                .All()
                .Where(m => m.Id == id)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
