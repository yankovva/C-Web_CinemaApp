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
