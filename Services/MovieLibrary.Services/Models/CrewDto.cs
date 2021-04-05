namespace MovieLibrary.Services.Models
{
    using Newtonsoft.Json;

    public class CrewDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }
    }
}
