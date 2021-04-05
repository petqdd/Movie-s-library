namespace MovieLibrary.Services.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class TrailerDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("results")]
        public List<TrailerKeyDto> TrailerResults { get; set; }
    }
}
