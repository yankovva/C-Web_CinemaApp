using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.MovieViewModels
{
    public class MovieDetailsViewModel
    {
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string ReleaseDate { get; set; } = null!;
        public string Director { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Description { get; set; } = null!;

    }
}
