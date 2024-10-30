using CinemaApp.Web.ViewModels.CinemaViewModels;
using System.ComponentModel.DataAnnotations;
using static CinemaApp.Web.Common.EntityValidatonConstants.Movie;

namespace CinemaApp.Web.ViewModels.MovieViewModels
{
    public class AddMovieToCinemaInputModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MaxLength(TitleMaxLenght)]
        public string Title { get; set; } = null!;

        public IList<CinemaCheckBoxItemInputModel> Cinemas { get; set; } = new List<CinemaCheckBoxItemInputModel>();
    }
}
