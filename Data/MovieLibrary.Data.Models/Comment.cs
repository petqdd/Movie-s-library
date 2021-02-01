namespace MovieLibrary.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public Comment()
        {
            this.Users = new HashSet<UsersComment>();
            this.Movies = new HashSet<MoviesComment>();
        }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<UsersComment> Users { get; set; }

        public virtual ICollection<MoviesComment> Movies { get; set; }
    }
}
