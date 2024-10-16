using CinemaApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using  CinemaWeb.Data.Models;
using CinemaApp.Web.ViewModels.Movie;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Web.ViewModels.Cinema;

namespace CinemaApp.Web.Controllers
{
    public class MovieController : Controller
    {
        //Dependecy Injection
        private readonly CinemaDbContext dbContext;

        public MovieController(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Movie> allMovies =await  this.dbContext
                .Movies
                .ToListAsync();

            //подаваме АлМувис ако обект на вюто.
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
            bool IsValid = Guid.TryParse(id, out Guid guidId);

            if (!IsValid) {
                return this.RedirectToAction(nameof(Index));
            }

            Movie? movie = await this.dbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == guidId);

            if (movie == null) {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> AddToProgram(string? id)
        {
            //проверка дали има нещо поддадено в УРЛ-а
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.RedirectToAction(nameof(Index));
            }

            //проверка дали нещото в УРЛ-а е Гуид
            bool isGuidValid = Guid.TryParse(id, out var movieGuid);
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
                    .Any(cm => cm.Movie.Id == movieGuid)
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
            //проверка дали има нещо поддадено в УРЛ-а
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                return this.RedirectToAction(nameof(Index));
            }

            //проверка дали нещото в УРЛ-а е Гуид
            bool isGuidValid = Guid.TryParse(model.Id, out var movieGuid);
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

