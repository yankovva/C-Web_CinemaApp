using CinemaApp.Data;
using CinemaApp.Web.ViewModels.Cinema;
using CinemaApp.Web.ViewModels.Movie;
using CinemaWeb.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CinemaApp.Web.Controllers
{
    public class CinemaController : Controller
    {

        private readonly CinemaDbContext dbContext;

        public CinemaController(CinemaDbContext context)
        {
                this.dbContext = context;
        }

        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await this.dbContext.Cinemas
                .Select( c=> new CinemaIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,

                })
                .OrderBy( c => c.Location)
                .ToArrayAsync();
            
            return View(cinemas);
        }

        [HttpGet]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Create()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (CreateCinemaVIewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            Cinema cinema = new Cinema()
            {
                Name = model.Name,
                Location = model.Location,
            };

           await this.dbContext.Cinemas.AddAsync(cinema);
           await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(string? id)
        {
            //проверка дали има нещо поддадено в УРЛ-а
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.RedirectToAction(nameof(Index));
            }

            //проверка дали нещото в УРЛ-а е Гуид
            bool isGuidValid = Guid.TryParse(id, out var cinemaGuid);
            if (!isGuidValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            Cinema? cinema = await this.dbContext.Cinemas
                .Include(c=> c.CinemaMovies)
                .ThenInclude(c=>c.Movie)
                .FirstOrDefaultAsync(c => c.Id == cinemaGuid);

            //Проверка дали има Кино с Такъв Гуид
            if (cinema == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            CinemaDetailsViewModel model = new CinemaDetailsViewModel()
            {
                Name = cinema.Name,
                Location = cinema.Location,
                Movies = cinema.CinemaMovies
                .Where(cm=>cm.IsDeleted == false)
                .Select(cm => new CinemaMovieViewModel()
                {
                    Title = cm.Movie.Title,
                    Duration = cm.Movie.Duration
                })
                .ToArray()
            };

            return this.View(model);

        }
    }
}
