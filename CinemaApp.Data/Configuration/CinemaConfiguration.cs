using CinemaWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CinemaApp.Web.Common.EntityValidatonConstants.Cinema;

namespace CinemaApp.Data.Configuration
{
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLenght);

            builder.Property(c => c.Location)
                .IsRequired()
                .HasMaxLength(LocationMaxLenght);

            builder.HasData(this.GenerateCinemas());
        }

        private IEnumerable<Cinema> GenerateCinemas()
        {
            IEnumerable<Cinema> cinemas = new List<Cinema>
            {
                new Cinema()
                {
                    Name = "Cinema City",
                    Location = "Plovdiv"
                },
                new Cinema()
                {
                    Name = "Cinema Lorld",
                    Location = "Karlovo"
                },
                new Cinema()
                {
                    Name = "Cinemax",
                    Location = "Varna"
                }
            };

            return cinemas;
        }
    }


}
