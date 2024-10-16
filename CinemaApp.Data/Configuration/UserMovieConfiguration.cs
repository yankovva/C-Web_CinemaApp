using CinemaWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Configuration
{
    public class UserMovieConfiguration : IEntityTypeConfiguration<UserMovie>
    {
        public void Configure(EntityTypeBuilder<UserMovie> builder)
        {
            builder.HasKey(um => new { um.MovieId, um.ApplicationUserId });

            builder.HasOne(um => um.Movie)
                .WithMany(um=>um.MoviesApplicationUsers)
                .HasForeignKey(um => um.MovieId);


            builder.HasOne(um => um.User)
                .WithMany(um=>um.ApplicationUsersMovies)
                .HasForeignKey(um => um.ApplicationUserId);

        }
    }
}
