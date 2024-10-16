using CinemaWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Configuration
{
    public class CinemaMovieConfiguration : IEntityTypeConfiguration<CinemaMovie>
    {
        public void Configure(EntityTypeBuilder<CinemaMovie> builder)
        {
            //правим композитен ключ
            builder.HasKey(cm => new
            {
                cm.MovieId,
                cm.CinemaId,
            });

            builder.Property(cm => cm.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(cm => cm.Movie)
                .WithMany(cm=>cm.CinemaMovies)
                .HasForeignKey(cm=>cm.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cm => cm.Cinema)
               .WithMany(cm => cm.CinemaMovies)
               .HasForeignKey(cm => cm.CinemaId)
               .OnDelete(DeleteBehavior.Restrict);

        }

        
    }
}
