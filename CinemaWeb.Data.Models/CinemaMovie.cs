﻿namespace CinemaWeb.Data.Models
{
    public class CinemaMovie
    {

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public Guid CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;

        //ne mi go slaga v DB????????? >.<
        public bool IsDeleted { get; set; } = false;
    }
}