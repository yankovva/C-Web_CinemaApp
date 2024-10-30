using System.ComponentModel.DataAnnotations;
using static CinemaApp.Web.Common.EntityValidatonConstants.Movie;

namespace CinemaApp.Web.ViewModels.MovieViewModels
{
    public class AddMovieFormModel
    {

        public AddMovieFormModel()
        {
            this.ReleaseDate = DateTime.Today;
        }

        [Required(ErrorMessage ="Title is required.")]
        [MaxLength(TitleMaxLenght)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage ="Genre is required.")]
        [MaxLength(TitleMaxLenght)]
        [MinLength(GenreMinLenght)]
        public string Genre { get; set; } = null!;

        [Required]
        public DateTime ReleaseDate { get; set; } 

        [Range(DurationMinValue, DurationMaxValue)]
        public int Duration { get; set; }

        [Required(ErrorMessage ="Director name is required.")]
        [MaxLength(DirectorMaxLEnght)]
        [MinLength(DirectorMinLEnght)]
        public string Director { get; set; } = null!;

        [Required(ErrorMessage ="Movie description is required and at least 50characters.")]
        [MaxLength(DescriptionMaxLenght)]
        [MinLength(DescriptionMinLenght)]
        public string Description { get; set; } = null!;

        [MaxLength(URLMaxLEnght)]
        public string? ImageURL { get; set; }
    }
}
