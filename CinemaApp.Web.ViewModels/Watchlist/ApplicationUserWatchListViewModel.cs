using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Watchlist
{
    public class ApplicationUserWatchListViewModel
    {
        public string ImageUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string MovieId { get; set; } = null!;

    }
}
