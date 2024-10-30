using CinemaApp.Web.ViewModels.MovieViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesViewModel>> IndexGetAllMovies();
        Task AddMovieAsync(AddMovieFormModel inputModel);

        Task<MovieDetailsViewModel> GetMovieDetailsById(Guid id);
        Task<AddMovieToCinemaInputModel> GetMovieToCinemaByIdAsync(Guid id);

        Task<bool> AddMovieToCinemaAsync(AddMovieToCinemaInputModel model, Guid id);
    }
}
