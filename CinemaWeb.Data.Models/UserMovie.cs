using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaWeb.Data.Models
{
    public class UserMovie
    {
        public Guid ApplicationUserId { get; set; } 
        public virtual ApplicationUser User { get; set; } = null!;

        public Guid MovieId { get; set; } 
        public Movie Movie { get; set; } = null!;
    }
}
