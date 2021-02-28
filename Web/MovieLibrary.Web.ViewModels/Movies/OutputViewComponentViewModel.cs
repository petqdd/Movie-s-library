namespace MovieLibrary.Web.ViewModels.Movies
{
    using System;

    public class OutputViewComponentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public string PosterPath { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
