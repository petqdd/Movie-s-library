namespace MovieLibrary.Data.Models
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Artist : BaseDeletableModel<int>
    {
        public Artist()
        {
            this.Movies = new HashSet<MoviesArtist>();
        }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string BiographyUrl { get; set; }

        public string PhotoUrl { get; set; }

        public virtual ICollection<MoviesArtist> Movies { get; set; }
    }
}
