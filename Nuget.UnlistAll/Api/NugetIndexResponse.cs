using Newtonsoft.Json;

namespace Nuget.UnlistAll.Api
{
    public class NugetIndexResponse
    {
        [JsonProperty("versions")]
        public string[] Versions { get; set; }
    }
}
