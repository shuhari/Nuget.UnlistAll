using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuget.UnlistAll.Models;

namespace Nuget.UnlistAll.Tasks
{
    public class UnlistTask : WorkerTask
    {
        public UnlistTask(IWorkerUi ui, NugetParams parameters, PackageVersionInfo versions) 
            : base(ui, parameters)
        {
            _versions = versions;
        }

        private readonly PackageVersionInfo _versions;

        protected override object ExecuteCore()
        {
            /*
            var arguments = $"delete {package.Id} {package.Version} -ApiKey {apiKey} -Source https://api.nuget.org/v3/index.json -NonInteractive";
            var psi = new ProcessStartInfo(@"D:\oss\github\Shuhari.Framework\packages\NuGet.CommandLine.3.5.0\tools\nuget.exe", arguments)
            {
                RedirectStandardOutput = true,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                UseShellExecute = false
            };
            var process = Process.Start(psi);
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();

            */
            return null;
        }
    }
}
