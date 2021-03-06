﻿namespace THECinema.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using THECinema.Data.Common.Repositories;
    using THECinema.Data.Models;
    using THECinema.Services.Data.Contracts;
    using THECinema.Services.Mapping;
    using THECinema.Web.ViewModels.Movies;

    public class MoviesService : IMoviesService
    {
        private const string InvalidIdExceptionMessage = "Movie doesn't exist!";

        private readonly IDeletableEntityRepository<Movie> moviesRepository;

        public MoviesService(IDeletableEntityRepository<Movie> moviesRepository)
        {
            this.moviesRepository = moviesRepository;
        }

        public async Task AddAsync(AddMovieInputModel model)
        {
            var movie = new Movie
            {
                Name = model.Name,
                Year = model.Year,
                Actors = model.Actors,
                AgeRestriction = model.AgeRestriction,
                Description = model.Description,
                Director = model.Director,
                Duration = model.Duration,
                Rating = model.Rating,
                Price = model.Price,
                Genre = model.Genre,
                TrailerUrl = model.TrailerUrl,
                TrailerVideoUrl = model.TrailerVideoUrl,
                Halls = null,
            };

            await this.moviesRepository.AddAsync(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int filmId)
        {
            var movie = this.moviesRepository.All().Where(m => m.Id == filmId).FirstOrDefault();

            if (movie == null)
            {
                throw new ArgumentNullException(InvalidIdExceptionMessage);
            }

            this.moviesRepository.Delete(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(AddMovieInputModel inputModel)
        {
            var movie = await this.moviesRepository.GetByIdWithDeletedAsync(inputModel.Id);

            if (movie == null)
            {
                throw new ArgumentNullException(InvalidIdExceptionMessage);
            }

            movie.Name = inputModel.Name;
            movie.Year = inputModel.Year;
            movie.Actors = inputModel.Actors;
            movie.AgeRestriction = inputModel.AgeRestriction;
            movie.Description = inputModel.Description;
            movie.Director = inputModel.Director;
            movie.Duration = inputModel.Duration;
            movie.Rating = inputModel.Rating;
            movie.Price = inputModel.Price;
            movie.Genre = inputModel.Genre;
            movie.TrailerUrl = inputModel.TrailerUrl;
            movie.TrailerVideoUrl = inputModel.TrailerVideoUrl;

            this.moviesRepository.Update(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int? take, int skip = 0)
        {
            var query = this.moviesRepository
                .All()
                .OrderByDescending(m => m.CreatedOn)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(int filmId)
        {
            return this.moviesRepository
                .All()
                .Where(m => m.Id == filmId)
                .To<T>()
                .FirstOrDefault();
        }

        public int GetIdByName(string filmName)
        {
            return this.moviesRepository
                .All()
                .Where(m => m.Name == filmName)
                .Select(m => m.Id)
                .FirstOrDefault();
        }

        public int GetMoviesCount()
        {
            return this.moviesRepository.All().Count();
        }
    }
}
