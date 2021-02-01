namespace MovieLibrary.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Rating : BaseDeletableModel<int>
    {
        public Rating()
        {
            this.Users = new HashSet<UsersRatings>();
            this.Movies = new HashSet<MoviesRatings>();
        }

        public int Vote { get; set; }

        public DateTime CreatedData { get; set; }

        public virtual ICollection<UsersRatings> Users { get; set; }

        public virtual ICollection<MoviesRatings> Movies { get; set; }
    }
}
