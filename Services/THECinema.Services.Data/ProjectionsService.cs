namespace THECinema.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Projections;

    public class ProjectionsService : IProjectionsService
    {
        private readonly IDeletableEntityRepository<Projection> projectionsRepository;
        private readonly IDeletableEntityRepository<ProjectionSeat> projectionsSeatsRepo;
        private readonly IDeletableEntityRepository<Seat> seatsRepository;

        public ProjectionsService(
            IDeletableEntityRepository<Projection> projectionsRepository,
            IDeletableEntityRepository<ProjectionSeat> projectionsSeatsRepo,
            IDeletableEntityRepository<Seat> seatsRepository)
        {
            this.projectionsRepository = projectionsRepository;
            this.projectionsSeatsRepo = projectionsSeatsRepo;
            this.seatsRepository = seatsRepository;
        }

        public async Task AddAsync(AddProjectionInputModel inputModel)
        {
            var projectionSeats = new List<ProjectionSeat>();

            var projection = new Projection
            {
                Id = Guid.NewGuid().ToString(),
                HallId = inputModel.HallId,
                MovieId = inputModel.MovieId,
                ProjectionDateTime = inputModel.ProjectionDateTime.ToUniversalTime(),
                Seats = projectionSeats,
            };

            var seats = this.seatsRepository
                .All()
                .Where(s => s.HallId == inputModel.HallId)
                .ToList();

            foreach (var seat in seats)
            {
                var projectionSeat = new ProjectionSeat
                {
                    Id = Guid.NewGuid().ToString(),
                    SeatId = seat.Id,
                    ProjectionId = projection.Id,
                };

                projectionSeats.Add(projectionSeat);
            }

            await this.projectionsRepository.AddAsync(projection);
            await this.projectionsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var projection = this.projectionsRepository.All().Where(p => p.Id == id).FirstOrDefault();
            this.projectionsRepository.Delete(projection);
            await this.projectionsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(AddProjectionInputModel inputModel)
        {
            var projection = await this.projectionsRepository.GetByIdWithDeletedAsync(inputModel.Id);

            if (projection.HallId != inputModel.HallId)
            {
                var oldSeats = this.projectionsSeatsRepo.All().Where(s => s.ProjectionId == inputModel.Id).ToList();

                foreach (var oldSeat in oldSeats)
                {
                    this.projectionsSeatsRepo.HardDelete(oldSeat);
                }

                await this.projectionsSeatsRepo.SaveChangesAsync();

                var projectionSeats = new List<ProjectionSeat>();
                var seats = this.seatsRepository
                .All()
                .Where(s => s.HallId == inputModel.HallId)
                .ToList();

                foreach (var seat in seats)
                {
                    var projectionSeat = new ProjectionSeat
                    {
                        Id = Guid.NewGuid().ToString(),
                        SeatId = seat.Id,
                        ProjectionId = projection.Id,
                    };

                    projectionSeats.Add(projectionSeat);
                }

                projection.Seats = projectionSeats;
            }

            projection.HallId = inputModel.HallId;
            projection.MovieId = inputModel.MovieId;
            projection.ProjectionDateTime = inputModel.ProjectionDateTime.ToUniversalTime();

            this.projectionsRepository.Update(projection);
            await this.projectionsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.projectionsRepository
               .All()
               .To<T>()
               .ToList();
        }

        public IEnumerable<T> GetById<T>(int filmId)
        {
            return this.projectionsRepository
                 .All()
                 .Where(p => p.MovieId == filmId)
                 .To<T>()
                 .ToList();
        }

        public T GetByProjectionId<T>(string id)
        {
            return this.projectionsRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
