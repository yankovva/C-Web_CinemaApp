using CinemaApp.Data;
using CinemaApp.Web.ViewModels.Watchlist;
using CinemaWeb.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{
    public class WatchListController : Controller
    {
        //Dependecy Injection
        private readonly CinemaDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;


        public WatchListController(CinemaDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            string? userId = userManager.GetUserId(User)!;

            //if (string.IsNullOrEmpty(userId))
            //{
            //    return RedirectToRoute("/Identity/Account/Login");
            //}

            IEnumerable<ApplicationUserWatchListViewModel> watchList = await this.dbContext
                .UsersMovies
                .Include(um=>um.Movie)
                .Where(um=>um.ApplicationUserId.ToString().ToLower() == userId.ToLower())
                .Select(um=> new ApplicationUserWatchListViewModel
                {
                    MovieId = um.MovieId.ToString(),
                    Title = um.Movie.Title,
                    Genre = um.Movie.Genre,
                    ReleaseDate = um.Movie.ReleaseDate,
                    ImageUrl = um.Movie.ImageURL
                    

                }).ToListAsync();

            return View(watchList);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToWatchList(string? movieId)
        {
            //проверка дали има нещо поддадено в УРЛ-а
            if (string.IsNullOrWhiteSpace(movieId))
            {
                return this.RedirectToAction(nameof(Index), "Movie");
            }

            //проверка дали нещото в УРЛ-а е Гуид
            bool isGuidValid = Guid.TryParse(movieId, out var movieGuid);
            if (!isGuidValid)
            {
                return this.RedirectToAction(nameof(Index), "Movie");
            }

            Movie? movie = await this.dbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction(nameof(Index), "Movie");
            }

            Guid userguid = Guid.Parse(this.userManager.GetUserId(User)!);

            bool addedTOWatchlistAlready = await this.dbContext
                .UsersMovies.AnyAsync(um => um.ApplicationUserId == userguid && um.MovieId == movieGuid);

            if (!addedTOWatchlistAlready)
            {
                UserMovie newUserMovie = new UserMovie()
                {
                    ApplicationUserId = userguid,
                    MovieId = movieGuid,
                };

                await this.dbContext.UsersMovies.AddAsync(newUserMovie);
                await this.dbContext.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromWatchlist(string? movieId)
        {
            //проверка дали има нещо поддадено в УРЛ-а
            if (string.IsNullOrWhiteSpace(movieId))
            {
                return this.RedirectToAction(nameof(Index), "Movie");
            }

            //проверка дали нещото в УРЛ-а е Гуид
            bool isGuidValid = Guid.TryParse(movieId, out var movieGuid);
            if (!isGuidValid)
            {
                return this.RedirectToAction(nameof(Index), "Movie");
            }

            Movie? movie = await this.dbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction(nameof(Index), "Movie");
            }

            Guid userguid = Guid.Parse(this.userManager.GetUserId(User)!);

            UserMovie? UserMovie = await this.dbContext
               .UsersMovies
               .FirstOrDefaultAsync(um => um.ApplicationUserId == userguid && um.MovieId == movieGuid);

            if(UserMovie != null)
            {
                this.dbContext.UsersMovies
                    .Remove(UserMovie);
                await this.dbContext.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
