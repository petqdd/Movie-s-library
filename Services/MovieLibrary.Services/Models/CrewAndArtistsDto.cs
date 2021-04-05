namespace MovieLibrary.Services.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class CrewAndArtistsDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("cast")]
        public List<ArtistDto> Artists { get; set; }

        [JsonProperty("crew")]
        public List<CrewDto> Crew { get; set; }
    }
}
