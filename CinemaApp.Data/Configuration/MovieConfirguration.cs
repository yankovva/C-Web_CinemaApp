using CinemaWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CinemaApp.Web.Common.EntityValidatonConstants.Movie;
using static CinemaApp.Web.Common.ApplicationConstants;


namespace CinemaApp.Data.Configuration
{
    internal class MovieConfirguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            //Fluent API
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(TitleMaxLenght);

            builder.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(GenreMaxLenght);

            builder.Property(m => m.Director)
                .IsRequired()
                .HasMaxLength(DirectorMaxLEnght);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLenght);

            builder.Property(m => m.ImageURL)
                .IsRequired(false)
                .HasMaxLength(URLMaxLEnght)
                .HasDefaultValue(NoImagePhotoURL);
            

           builder.HasData(this.SeedMovies());

        }

        private List<Movie> SeedMovies()
        {
            List<Movie> movies = new List<Movie>()
            {
                new Movie()
                {
                    Title = "Harry Poter",
                    Genre = "Fantasy",
                    Director ="Mike Newel",
                    Duration = 157,
                    ReleaseDate = new DateTime(2005, 11, 01),
                    Description = "An amazing movie"
                },
                 new Movie()
                {
                    Title = "Lord of the Rings",
                    Genre = "Fantasy",
                    Director ="Peter Huston",
                    Duration = 178,
                    ReleaseDate = new DateTime(2001, 05, 01),
                    Description = "An amazing movie"
                }
            };
            return movies;

        } 
    }
}
