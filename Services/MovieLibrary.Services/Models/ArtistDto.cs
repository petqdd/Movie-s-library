namespace MovieLibrary.Services.Models
{
    using Newtonsoft.Json;

    public class ArtistDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profile_path")]
        public string PhotoUrl { get; set; }
    }
}
