using System.ComponentModel.DataAnnotations;
using static CinemaApp.Web.Common.EntityValidatonConstants.Cinema;

namespace CinemaApp.Web.ViewModels.CinemaViewModels
{
    public class CinemaCheckBoxItemInputModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(NameMinLenght)]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(LocationMinLenght)]
        [MaxLength(LocationMaxLenght)]
        public string Location { get; set; } = null!;

        public bool IsSelected { get; set; }
    }
}
