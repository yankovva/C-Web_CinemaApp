using CinemaApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using  CinemaWeb.Data.Models;
using CinemaApp.Web.ViewModels.MovieViewModels;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Web.ViewModels.CinemaViewModels;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Data.Services.Interfaces;

namespace CinemaApp.Web.Controllers
{
    public class MovieController : BaseController
    {
        //Dependecy Injection
        private readonly CinemaDbContext dbContext;
        private readonly IMovieService movieService;

        public MovieController(CinemaDbContext dbContext, IMovieService movieService)
        {
            this.dbContext = dbContext;
            this.movieService = movieService;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           IEnumerable<AllMoviesViewModel> allMovies = await this.movieService
                 .IndexGetAllMovies();

            return View(allMovies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddMovieFormModel inputModel)
        {

            //bool isReleasedateValid = DateTime.TryParseExact(inputModel.ReleaseDate, "MMMM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowInnerWhite,
            //    out DateTime releaseDate);

            //if (!isReleasedateValid)
            //{
            //    this.ModelState.AddModelError(nameof(inputModel.ReleaseDate), "The release date must be in the following format MMM/yyyy");

            //}
            if (!this.ModelState.IsValid)
            {
                //Rendering the same form with the same input values + model errors will be shown 
                return this.View(inputModel);
            }

            Movie movie = new Movie()
            {
                Title = inputModel.Title,
                Genre = inputModel.Genre,
                ReleaseDate = inputModel.ReleaseDate,
                Director = inputModel.Director,
                Duration = inputModel.Duration,
                Description = inputModel.Description,
                ImageURL= inputModel.ImageURL,
            };

             await this.dbContext.Movies.AddAsync(movie);
             await this.dbContext.SaveChangesAsync();

            return  this.RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Guid movieGuid = Guid.Empty;
            bool isGuidValid = this.IsGuidValid(id, ref movieGuid);
            if (!isGuidValid)
            {
                // Invalid id format
                return this.RedirectToAction(nameof(Index));
            }

            Movie? movie = await this.dbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null) {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> AddToProgram(string? id)
        {
            

            Guid movieGuid = Guid.Empty;
            bool isGuidValid = this.IsGuidValid(id, ref movieGuid);
            if (!isGuidValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Movie? movie = await this.dbContext.Movies
                .FirstOrDefaultAsync(m=>m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }

            AddMovieToCinemaInputModel model = new AddMovieToCinemaInputModel()
            {
                Id = movie.Id.ToString(),
                Title = movie.Title,
                Cinemas = await this.dbContext.Cinemas
                .Include(cm => cm.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .Select(c => new CinemaCheckBoxItemInputModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,
                    IsSelected = c.CinemaMovies
                    .Any(cm => cm.Movie.Id == movieGuid && cm.IsDeleted == false)
                })
                .ToArrayAsync()
            };

            return this.View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddToProgram(AddMovieToCinemaInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            Guid movieGuid = Guid.Empty;
            bool isGuidValid = this.IsGuidValid(model.Id, ref movieGuid);
            if (!isGuidValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Movie? movie = await this.dbContext.Movies
               .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ICollection<CinemaMovie> CinemaMoviesToAdd = new List<CinemaMovie>();

            foreach (CinemaCheckBoxItemInputModel cinemaInputModel in model.Cinemas)
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(cinemaInputModel);
                }

                if (string.IsNullOrWhiteSpace(cinemaInputModel.Id))
                {
                    return this.RedirectToAction(nameof(Index));
                }

                bool isCinemaGuidValid = Guid.TryParse(cinemaInputModel.Id, out var cinemaGuid);
                if (!isCinemaGuidValid)
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid Operation!");
                    return this.View(model);
                }

                Cinema? cinema = await this.dbContext.Cinemas
                .FirstOrDefaultAsync(c => c.Id == cinemaGuid);

                if (cinema == null)
                {
                    this.ModelState.AddModelError(string.Empty, "No Cinema Found!");
                    return this.View(model);
                }

                CinemaMovie? cinemasMovies = await this.dbContext.CinemasMovies
                        .FirstOrDefaultAsync(cm => cm.MovieId == movieGuid && cm.CinemaId == cinemaGuid);

                if (cinemaInputModel.IsSelected)
                {
                    if (cinemasMovies ==null)
                    {
                        CinemaMovie cinemaMovie = new CinemaMovie()
                        {
                            MovieId = movieGuid,
                            CinemaId = cinemaGuid,
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

                await this.dbContext.SaveChangesAsync();

            }
                await this.dbContext.CinemasMovies.AddRangeAsync(CinemaMoviesToAdd);
                await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof (Index),"Cinema");
        }
    }
}

