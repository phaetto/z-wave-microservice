namespace MicroServicesStarter.Deploy
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Chains;

    public sealed class PreparedDeploySetupContext : ChainWithParent<PreparedDeploySetupContext, DeploySetupContext>
    {
        public readonly string ReleaseNotes;

        public readonly string UpdateToVersion;

        public PreparedDeploySetupContext(
            DeploySetupContext deploySetupContext,
            string releaseNotes,
            ReleaseType releaseType)
            : base(deploySetupContext)
        {
            ReleaseNotes = releaseNotes;

            UpdateToVersion = GetNextVersion(releaseType, deploySetupContext.CurrentVersion);
        }

        public PreparedDeploySetupContext LogToUi(string text)
        {
            Parent.Parent.LogToUi(text);
            return this;
        }

        public static string GetNextVersion(ReleaseType releaseType, string currentVersion)
        {
            var versionNumbers = new int[0];

            try
            {
                versionNumbers = currentVersion.Split(
                    new[]
                    {
                        '.'
                    }).Select(int.Parse).ToArray();
            }
            catch (FormatException)
            {
            }

            if (versionNumbers.Length != 4)
            {
                throw new InvalidDataException(
                    "The version is not of the form x.x.x.x, where x numbers! Correct that in the version file");
            }

            switch (releaseType)
            {
                case ReleaseType.Debug:
                    ++versionNumbers[3];
                    break;
                case ReleaseType.Release:
                    ++versionNumbers[2];
                    versionNumbers[3] = 0;
                    break;
                case ReleaseType.Minor:
                    ++versionNumbers[1];
                    versionNumbers[2] = versionNumbers[3] = 0;
                    break;
                case ReleaseType.Major:
                    ++versionNumbers[0];
                    versionNumbers[1] = versionNumbers[2] = versionNumbers[3] = 0;
                    break;
            }

            return string.Join(".", versionNumbers);
        }
    }
}
