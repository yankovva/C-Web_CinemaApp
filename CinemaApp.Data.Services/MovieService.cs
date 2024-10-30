using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Data.Services.Interfaces;
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
	public class MovieService : BaseService, IMovieService
	{
		private readonly IRepository<Movie, Guid> movieRepository;
		private readonly IRepository<Cinema, Guid> cinemaRepository;
		private readonly IRepository<CinemaMovie, object> cinemaMovieRepository;


		public MovieService(IRepository<Movie, Guid> movieRepository, IRepository<Cinema, Guid> cinemaRepository, IRepository<CinemaMovie, object> cinemaMovieRepository)
		{
			this.movieRepository = movieRepository;
			this.cinemaRepository = cinemaRepository;
			this.cinemaMovieRepository = cinemaMovieRepository;

		}

		public async Task AddMovieAsync(AddMovieFormModel inputModel)
		{
			Movie movie = new Movie()
			{
				Title = inputModel.Title,
				Genre = inputModel.Genre,
				ReleaseDate = inputModel.ReleaseDate,
				Director = inputModel.Director,
				Duration = inputModel.Duration,
				Description = inputModel.Description,
				ImageURL = inputModel.ImageURL,
			};

			await movieRepository.AddAsync(movie);
		}

		public async Task<bool> AddMovieToCinemaAsync(AddMovieToCinemaInputModel model, Guid movieId)
		{
			Movie? movie = await movieRepository
				.GetByIdAsync(movieId);

			if (movie == null)
			{
				return false;
			}

			ICollection<CinemaMovie> CinemaMoviesToAdd = new List<CinemaMovie>();

			foreach (CinemaCheckBoxItemInputModel cinemaInputModel in model.Cinemas)
			{
				Guid cinemGuid = Guid.Empty;

				bool isCinemaGuidValid = this.IsGuidValid(cinemaInputModel.Id, ref cinemGuid);
				if (!isCinemaGuidValid)
				{
					return false;
				}

				Cinema? cinema = await this.cinemaRepository
					.GetAllAttached()
					.FirstOrDefaultAsync(c => c.Id == cinemGuid);

				if (cinema == null)
				{
					return false;
				}

				//CinemaMovie? cinemasMovies = await this.cinemaMovieRepository
				//	.GetAllAttached()
				//		.FirstOrDefaultAsync(cm => cm.MovieId == movieId && cm.CinemaId == cinemGuid);

				CinemaMovie? cinemasMovies = await this.cinemaMovieRepository
					.GetByIdAsync(movieId, cinemGuid);

				if (cinemaInputModel.IsSelected)
				{
					if (cinemasMovies == null)
					{
						CinemaMovie cinemaMovie = new CinemaMovie()
						{
							MovieId = movieId,
							CinemaId = cinemGuid,
						};

						CinemaMoviesToAdd.Add(cinemaMovie);
					}
					else
					{
						cinemasMovies.IsDeleted = false;
					}
				}
				else
				{
					if (cinemasMovies != null)
					{
						cinemasMovies.IsDeleted = true;
					}
				}

				//await this.dbContext.SaveChangesAsync();
			}
			await this.cinemaMovieRepository.AddRangeAsync(CinemaMoviesToAdd.ToArray());
			return true;
		}



		public async Task<MovieDetailsViewModel> GetMovieDetailsById(Guid id)
		{

			Movie? movie = await movieRepository
				.GetByIdAsync(id);

			MovieDetailsViewModel model = null;
			if (movie != null)
			{
				model = new MovieDetailsViewModel()
				{
					Title = movie.Title,
					Description = movie.Description,
					Duration = movie.Duration.ToString(),
					Director = movie.Director,
					ReleaseDate = movie.ReleaseDate.ToString("MMMM yyyy"),
					Genre = movie.Genre,

				};
			}
			return model;
		}

		public async Task<AddMovieToCinemaInputModel> GetMovieToCinemaByIdAsync(Guid id)
		{
			Movie? movie = await movieRepository
				.GetByIdAsync(id);

			AddMovieToCinemaInputModel model = null;
			if (movie != null)
			{
				model = new AddMovieToCinemaInputModel()
				{
					Id = id.ToString(),
					Title = movie.Title,
					Cinemas = await cinemaRepository
					.GetAllAttached()
						.Include(cm => cm.CinemaMovies)
						.ThenInclude(cm => cm.Movie)
						.Select(c => new CinemaCheckBoxItemInputModel()
						{
							Id = c.Id.ToString(),
							Name = c.Name,
							Location = c.Location,
							IsSelected = c.CinemaMovies
							.Any(cm => cm.Movie.Id == id && cm.IsDeleted == false)
						})
				.ToArrayAsync()
				};

			}
			return model;

		}

		public async Task<IEnumerable<AllMoviesViewModel>> IndexGetAllMovies()
		{
			IEnumerable<AllMoviesViewModel> movies = await movieRepository
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
