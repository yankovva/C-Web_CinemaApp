using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CinemaWeb.Data.Models
{
    public class Movie
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; } = null!;
        public int Duration { get; set; }
        public string Description { get; set; } = null!;

        public string? ImageURL { get; set; } 
        public virtual ICollection<CinemaMovie> CinemaMovies { get; set; } = new HashSet<CinemaMovie>();

        public virtual ICollection<UserMovie> MoviesApplicationUsers { get; set; } = new HashSet<UserMovie>();

    }
}
