using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Nuget.UnlistAll.Models;

namespace Nuget.UnlistAll
{
    public class NugetApi
    {
        public NugetApi(NugetParams parameters)
        {
            _parameters = parameters;
        }

        private readonly NugetParams _parameters;

        public NugetIndexResponse GetPackageVersions()
        {
            string url = string.Format("https://api.nuget.org/v3-flatcontainer/{0}/index.json",
                _parameters.PackageId.ToLowerInvariant());
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
