using System;
using System.IO;
using System.Linq;
using Nuget.UnlistAll.Models;
using System.Diagnostics;

namespace Nuget.UnlistAll.Tasks
{
    public class UnlistTask : WorkerTask
    {
        public UnlistTask(IWorkerUi ui, NugetParams parameters, PackageVersionInfo[] versions) 
            : base(ui, parameters)
        {
            _versions = versions;
        }

        private readonly PackageVersionInfo[] _versions;

        protected override object ExecuteCore()
        {
            // Inn developing mode, the .exe layout in bin\[Debug|Release], while 
            // when packaged by nuget the .exe are copied together with nuget.exe.
            // The task should support either case.
            var nugetPath = FindNugetPaths(@".\nuget.exe",
                @"..\..\..\packages\NuGet.CommandLine.4.1.0\tools\nuget.exe");
            if (nugetPath == null)
                throw new FileNotFoundException("Could not found nuget.exe");

            foreach (var version in _versions)
            {
                var cmdArgs = $"delete {version.PackageId} {version.Version} -ApiKey {Params.ApiKey} -Source https://api.nuget.org/v3/index.json -NonInteractive";
                var psi = new ProcessStartInfo(nugetPath, cmdArgs)
                {
                    RedirectStandardOutput = true,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                var process = Process.Start(psi);
                process.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                NotifyLog(true, $"unlist {version.PackageId} {version.Version} returns: {output}");
            }
            return null;
        }

        private string FindNugetPaths(params string[] relativePaths)
        {
            foreach (var relativePath in relativePaths)
            {
                var fullPath = FindNugetPath(relativePath);
                if (fullPath != null)
                    return fullPath;
            }
            return null;
        }

        private string FindNugetPath(string relativePath)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
            if (File.Exists(fullPath))
                return fullPath;
            return null;
        }
    }
}
