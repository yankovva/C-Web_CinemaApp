using CinemaApp.Data;
using CinemaApp.Data.Services.Interfaces;
using CinemaApp.Web.ViewModels.CinemaViewModels;
using CinemaApp.Web.ViewModels.MovieViewModels;
using CinemaWeb.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{
    public class CinemaController : BaseController
    {

        private readonly ICinemaService cinemaService;

        public CinemaController( ICinemaService cinemaService)
        {
                this.cinemaService = cinemaService;
        }

        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await this.cinemaService
                .IndexGetAllOrderedByLocationAsync();
                
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

            await this.cinemaService.AddCinemaAsync(model);

            return this.RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(string? id)
        {
            Guid cinemaGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref cinemaGuid);
            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }


            CinemaDetailsViewModel model = await  this.cinemaService
                .GetCinemaDetailsByIdAsync(cinemaGuid);

            //Проверка дали има Кино с Такъв Гуид
            if (model == null)
            {
                return this.RedirectToAction(nameof(Index));
            }
            return this.View(model);

        }
    }
}
