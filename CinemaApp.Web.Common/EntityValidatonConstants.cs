using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.Common
{
    public static class EntityValidatonConstants
    {
        public static class Movie
        {
            public const int TitleMaxLenght = 50;
            public const int GenreMaxLenght = 20;
            public const int GenreMinLenght = 5;
            public const int DirectorMaxLEnght = 80;
            public const int DirectorMinLEnght = 10;
            public const int DescriptionMaxLenght = 200;
            public const int DescriptionMinLenght = 50;
            public const int DurationMaxValue = 999;
            public const int DurationMinValue = 20;
            public const int URLMaxLEnght = 2083;
            public const int URLMinLenght = 8;


        }

        public static class Cinema
        {
            public const int NameMinLenght = 5;
            public const int NameMaxLenght = 20;
            public const int LocationMinLenght = 3;
            public const int LocationMaxLenght = 85;

        }
    }
}
