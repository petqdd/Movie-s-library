namespace MovieLibrary.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Director : BaseDeletableModel<int>
    {
        public Director()
        {
            this.Movies = new HashSet<Movie>();
        }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
