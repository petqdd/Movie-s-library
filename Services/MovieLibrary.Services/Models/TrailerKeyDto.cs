namespace MovieLibrary.Services.Models
{
    using Newtonsoft.Json;

    public class TrailerKeyDto
    {
        [JsonProperty("key")]
        public string TrailerKey { get; set; }
    }
}
