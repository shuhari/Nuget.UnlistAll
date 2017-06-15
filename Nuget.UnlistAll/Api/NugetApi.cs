using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Nuget.UnlistAll.Configuration;
using Shuhari.Framework.Utils;

namespace Nuget.UnlistAll.Api
{
    public class NugetApi
    {
        public NugetApi(AppConfig config)
        {
            Expect.IsNotNull(config, nameof(config));

            _config = config;
        }

        private readonly AppConfig _config;

        public NugetIndexResponse GetIndex()
        {
            string url = string.Format("https://api.nuget.org/v3-flatcontainer/{0}/index.json",
                _config.PackageId.ToLowerInvariant());
            var request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    string json = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
                    var result = JsonConvert.DeserializeObject<NugetIndexResponse>(json);
                    return result;
                }
            }
        }
    }
}
