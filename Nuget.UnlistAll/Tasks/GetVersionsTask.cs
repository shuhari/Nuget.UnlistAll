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
            NotifyLog(true, "Get version for package: {0}...", Params.PackageId);
            var response = new NugetApi(Params).GetPackageVersions();
            var result = response.Versions.Select(x => new PackageVersionInfo(Params.PackageId, x, true)).ToArray();
            NotifyLog(true, "Found {0} versions for Package {1}", result.Length, Params.PackageId);
            return result;
        }
    }
}
