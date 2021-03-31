namespace MovieLibrary.Data.Models
{
    using System;

    using MovieLibrary.Data.Common.Models;

    public class Photo : BaseModel<string>
    {
        public Photo()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string RemotePhotoUrl { get; set; }

        public string Extension { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
