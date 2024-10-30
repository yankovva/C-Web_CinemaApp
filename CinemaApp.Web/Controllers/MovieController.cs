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

            await this.movieService.AddMovieAsync(inputModel);

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

			MovieDetailsViewModel movie = await this.movieService
                .GetMovieDetailsById(movieGuid);
            if (movie == null)
            {
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

            AddMovieToCinemaInputModel model = await this.movieService
                .GetMovieToCinemaByIdAsync(movieGuid);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

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

            bool result = await this.movieService
                .AddMovieToCinemaAsync( model, movieGuid);
            if (result == false)
            {
				return this.RedirectToAction(nameof(Index));
			}


            return this.RedirectToAction(nameof (Index),"Cinema");
        }
    }
}

