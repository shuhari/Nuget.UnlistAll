using System.Linq;
using Nuget.UnlistAll.Models;

namespace Nuget.UnlistAll.Tasks
{
    public class GetVersionsTask : WorkerTask
    {
        public GetVersionsTask(IWorkerUi ui, NugetParams parameters) 
            : base(ui, parameters)
        {
        }

        protected override object ExecuteCore()
        {
            var response = new NugetApi(Params).GetPackageVersions();
            var result = response.Versions.Select(x => new PackageVersionInfo(Params.PackageId, x, true)).ToArray();
            return result;
        }
    }
}
