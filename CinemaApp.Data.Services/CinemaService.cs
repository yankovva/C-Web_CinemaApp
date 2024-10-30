using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Data.Services.Interfaces;
using CinemaApp.Services.Mapping;
using CinemaApp.Web.ViewModels.CinemaViewModels;
using CinemaApp.Web.ViewModels.MovieViewModels;
using CinemaWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly IRepository<Cinema, Guid> cinemaRepository;
        public CinemaService(IRepository<Cinema, Guid> cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }
        public async Task AddCinemaAsync(CreateCinemaVIewModel model)
        {
            Cinema cinema = new Cinema()
            {
                Name = model.Name,
                Location = model.Location
            };

            await this.cinemaRepository.AddAsync(cinema);
           
        }

        public async Task<CinemaDetailsViewModel> GetCinemaDetailsByIdAsync(Guid id)
        {
            Cinema? cinema = await this.cinemaRepository
                .GetAllAttached()
               .Include(c => c.CinemaMovies)
               .ThenInclude(c => c.Movie)
               .FirstOrDefaultAsync(c => c.Id == id);

            CinemaDetailsViewModel model = null;
            if (cinema != null)
            {
                model = new CinemaDetailsViewModel()
                {
                    Name = cinema.Name,
                    Location = cinema.Location,
                    Movies = cinema.CinemaMovies
               .Where(cm => cm.IsDeleted == false)
               .Select(cm => new CinemaMovieViewModel()
               {
                   Title = cm.Movie.Title,
                   Duration = cm.Movie.Duration
               })
               .ToArray()
                };
            }
            return model;
           
        }

        public async Task<IEnumerable<CinemaIndexViewModel>> IndexGetAllOrderedByLocationAsync()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await this.cinemaRepository
              .GetAllAttached()
              .OrderBy(c => c.Location)
              .To<CinemaIndexViewModel>()
              .ToArrayAsync();

            return cinemas;
        }
    }
}
