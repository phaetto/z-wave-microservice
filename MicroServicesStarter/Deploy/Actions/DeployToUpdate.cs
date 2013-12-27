namespace MicroServicesStarter.Deploy.Actions
{
    using System.IO;
    using Chains;
    using Chains.Play;
    using Chains.Play.Web;
    using Ionic.Zip;
    using MicroServicesStarter.Deploy.Actions.Update;
    using Services.Communication.Protocol;
    using Services.Packages.Update;

    class DeployToUpdate : IChainableAction<PreparedDeploySetupContext, PreparedDeploySetupContext>
    {
        private const string ArchiveFilename = "package-temp.zip";

        public PreparedDeploySetupContext Act(PreparedDeploySetupContext context)
        {
            foreach (var projectRegistration in context.Parent.Parent.Projects)
            {
                if (projectRegistration.IsUpdatePackage)
                {
                    var packageDirectory = string.Format(@"{0}Packages\{1}\", context.Parent.UpdateFolder, projectRegistration.Name);
                    var jsonData = File.ReadAllText(string.Format("{0}update.json", projectRegistration.Directory));

                    var packageUploadData = DeserializableSpecification<PackageUploadData>.DeserializeFromJson(jsonData);

                    packageUploadData.YourApiKey = context.Parent.UpdateApiKey;
                    packageUploadData.UpdateServerHostname = context.Parent.UpdateHost;
                    packageUploadData.UpdateServerPort = context.Parent.UpdatePort;
                    packageUploadData.PackageVersionLabel = context.UpdateToVersion;
                    packageUploadData.PackageFolder = packageDirectory;

                    context.LogToUi("Connecting to server...");
                    using (
                        var updateServer =
                            new Client(packageUploadData.UpdateServerHostname, packageUploadData.UpdateServerPort).Do(
                                new OpenConnection()))
                    {
                        context.LogToUi(string.Format("Zipping folder {0}...", packageUploadData.PackageFolder));
                        using (var zip = new ZipFile())
                        {
                            zip.AddDirectory(packageUploadData.PackageFolder);
                            zip.Save(ArchiveFilename);
                        }

                        context.LogToUi(string.Format("Uploading package {0}...", projectRegistration.Name));
                        updateServer.Do(
                            new Send(
                                new UploadPackage(
                                    new UploadPackageData
                                    {
                                        PackageToUpload = new PackageDescription
                                        {
                                            PackageName = packageUploadData.PackageName,
                                            VersionLabel = packageUploadData.PackageVersionLabel
                                        },
                                        ZipData = File.ReadAllBytes(ArchiveFilename)
                                    })
                                {
                                    ApiKey = packageUploadData.YourApiKey
                                }));

                        File.Delete(ArchiveFilename);

                        context.LogToUi(
                            string.Format(
                                "Package {0}, {1} has been successfully uploaded to tcp://{2}:{3}",
                                packageUploadData.PackageName,
                                packageUploadData.PackageVersionLabel,
                                packageUploadData.UpdateServerHostname,
                                packageUploadData.UpdateServerPort));
                    }
                }
            }

            return context;
        }
    }
}
