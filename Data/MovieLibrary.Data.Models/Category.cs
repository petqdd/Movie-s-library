namespace MovieLibrary.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Movies = new HashSet<Movie>();
        }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public int CountOfMovie => this.Movies.Count;
    }
}
