using System.ComponentModel.DataAnnotations;
using static CinemaApp.Web.Common.EntityValidatonConstants.Cinema;

namespace CinemaApp.Web.ViewModels.CinemaViewModels
{
    public class CreateCinemaVIewModel
    {
        [Required]
        [MaxLength(NameMaxLenght)]
        [MinLength(NameMinLenght)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(LocationMaxLenght)]
        [MinLength(LocationMinLenght)]
        public string Location { get; set; } = null!;
    }
}
