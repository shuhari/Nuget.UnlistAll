namespace Nuget.UnlistAll.Resources
{
    public static class Strings
    {
        public const string Title = "Nuget Unlist Tool";

        public const string SelectVersions = "Please select at least one version to unlist.";

        public const string ConfirmUnlist = "Are you sure to unlist {0} versions for package '{1}'?";

        public const string PackageIdRequired = "Package ID should not be empty";

        public const string ApiKeyRequired = "API Key should not be empty";

        public const string ListVersionsBegin = "List versions for package {0} begin...";

        public const string ListVersionsFinished = "List versions for package {0} finished, found {1} versions";

        public const string NugetNotFound = "Could not found nuget.exe";

        public const string NugetFound = "Found nuget.exe in {0}";

        public const string UnlistResult = "unlist {0} version {1} returns: {2}";

        public const string AboutTitle = "About Nuget Unlist Tool";
    }
}
