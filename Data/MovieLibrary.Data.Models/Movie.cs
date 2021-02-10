namespace MovieLibrary.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Movie : BaseDeletableModel<int>
    {
        public Movie()
        {
            this.Comments = new HashSet<MoviesComment>();
            this.Users = new HashSet<UsersMovie>();
            this.Artists = new HashSet<MoviesArtist>();
            this.Ratings = new HashSet<MoviesRatings>();
            this.Categories = new HashSet<MoviesCategory>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Year { get; set; }

        //public int CategoryId { get; set; }

        //public Category Category { get; set; }

        public int Runtime { get; set; }

        [Required]
        [MaxLength(200)]
        public string PosterPath { get; set; }

        public string SecondPosterPath { get; set; }

        [Required]
        [MaxLength(200)]
        public string TrailerUrl { get; set; }

        [Required]
        public string Storyline { get; set; }

        public double ImdbRating { get; set; }

        public double UserRating { get; set; }

        public DateTime CreatedDate { get; set; }

        public int DirectorId { get; set; }

        public Director Director { get; set; }

        public virtual ICollection<MoviesCategory> Categories { get; set; }

        public virtual ICollection<MoviesComment> Comments { get; set; }

        public virtual ICollection<UsersMovie> Users { get; set; }

        public virtual ICollection<MoviesRatings> Ratings { get; set; }

        public virtual ICollection<MoviesArtist> Artists { get; set; }
    }
}
