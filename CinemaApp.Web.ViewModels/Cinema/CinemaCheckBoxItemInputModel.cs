using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CinemaApp.Web.Common.EntityValidatonConstants.Cinema;

namespace CinemaApp.Web.ViewModels.Cinema
{
    public class CinemaCheckBoxItemInputModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(NameMinLenght)]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(LocationMinLenght)]
        [MaxLength(LocationMaxLenght)]
        public string Location { get; set; } = null!;

        public bool IsSelected { get; set; }
    }
}
