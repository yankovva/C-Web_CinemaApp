
using CinemaApp.Web.ViewModels.CinemaViewModels;

namespace CinemaApp.Data.Services.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaIndexViewModel>> IndexGetAllOrderedByLocationAsync();

        Task AddCinemaAsync(CreateCinemaVIewModel model);
        Task<CinemaDetailsViewModel> GetCinemaDetailsByIdAsync(Guid id);
    }
}
