using Newtonsoft.Json;

namespace Nuget.UnlistAll.Models
{
    public class NugetIndexResponse
    {
        [JsonProperty("versions")]
        public string[] Versions { get; set; }
    }
}
