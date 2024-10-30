using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Data.Services.Interfaces;
using CinemaApp.Services.Mapping;
using CinemaApp.Web.ViewModels.CinemaViewModels;
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
        public Task AddCinemaAsync(CreateCinemaVIewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<CinemaDetailsViewModel> GetCinemaDetailsByIdAsync(Guid id)
        {
            throw new NotImplementedException();
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
