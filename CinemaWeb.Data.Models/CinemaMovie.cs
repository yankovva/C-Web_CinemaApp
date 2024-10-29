namespace CinemaWeb.Data.Models
{
    public class CinemaMovie
    {

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public Guid CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;
    }
}