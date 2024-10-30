using CinemaApp.Services.Mapping;
using CinemaWeb.Data.Models;

namespace CinemaApp.Web.ViewModels.CinemaViewModels
{
    public class CinemaIndexViewModel : IMapFrom<Cinema>
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;

    }
}
