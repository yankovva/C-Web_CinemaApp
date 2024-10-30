using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Web.ViewModels.MovieViewModels;
using CinemaWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Services.Interfaces
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie,Guid> movieRepository;
        public MovieService(IRepository<Movie, Guid> movieRepository)
        {
            this.movieRepository = movieRepository;
        }
        public async Task<IEnumerable<AllMoviesViewModel>> IndexGetAllMovies()
        {
            IEnumerable<AllMoviesViewModel> movies = await this.movieRepository
                .GetAllAttached()
                .Select(m => new AllMoviesViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Director = m.Director,
                    Duration = m.Duration.ToString(),
                    ReleaseDate = m.ReleaseDate.ToString("MMMM yyyy")
                })
                .ToArrayAsync();

            return movies;
        }
    }
}
