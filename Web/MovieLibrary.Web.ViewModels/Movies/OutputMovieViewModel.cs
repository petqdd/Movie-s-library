﻿using System.Collections.Generic;

namespace MovieLibrary.Web.ViewModels.Movies
{
    public class OutputMovieViewModel
    {
        public OutputMovieViewModel()
        {
            this.Categories = new HashSet<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public string PosterPath { get; set; }

        //public int Runtime { get; set; }

        //public string TrailerUrl { get; set; }

        //public string Storyline { get; set; }

        //public double ImdbRating { get; set; }

        //public double UserRating { get; set; }

        public bool CollectIsNotAvailable { get; set; }

        public ICollection<string> Categories { get; set; }
    }
}
