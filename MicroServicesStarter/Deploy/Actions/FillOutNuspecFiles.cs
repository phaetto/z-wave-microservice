namespace MicroServicesStarter.Deploy.Actions
{
    using System.IO;
    using System.Xml;
    using Chains;

    public sealed class FillOutNuspecFiles : IChainableAction<PreparedDeploySetupContext, PreparedDeploySetupContext>
    {
        public PreparedDeploySetupContext Act(PreparedDeploySetupContext context)
        {
            foreach (var projectRegistration in context.Parent.Parent.Projects)
            {
                if (!projectRegistration.IsNugetPackage)
                {
                    continue;
                }

                var binNuspecFile = string.Format("{0}{1}.nuspec", projectRegistration.Directory, projectRegistration.Name);

                var packageDirectory = string.Format(@"{0}Packages\{1}\", context.Parent.NugetFolder, projectRegistration.Name);

                var newNuspecFile = packageDirectory + Path.GetFileName(binNuspecFile);

                var nuspecText = File.ReadAllText(binNuspecFile);

                nuspecText = nuspecText.Replace("{Version}", context.UpdateToVersion);
                nuspecText = nuspecText.Replace("{ReleaseNotes}", context.ReleaseNotes);

                File.WriteAllText(newNuspecFile, nuspecText);

                var packagesConfigFile = string.Format("{0}packages.config", projectRegistration.Directory);

                // Update the packages information
                if (File.Exists(packagesConfigFile))
                {
                    var nugetXmlDocument = new XmlDocument();
                    nugetXmlDocument.Load(newNuspecFile);

                    var packagesXmlDocument = new XmlDocument();
                    packagesXmlDocument.Load(packagesConfigFile);
                    var packageNodes = packagesXmlDocument.SelectNodes("/packages/package");
                    foreach (XmlNode packageNode in packageNodes)
                    {
                        var id = packageNode.Attributes["id"].Value;
                        var version = packageNode.Attributes["version"].Value;

                        var nugetPackageNode = nugetXmlDocument.SelectSingleNode(string.Format("/package/metadata/dependencies/dependency[@id='{0}']", id));

                        if (nugetPackageNode != null && nugetPackageNode.Attributes["version"].Value != version)
                        {
                            nugetPackageNode.Attributes["version"].Value = version;
                        }
                    }

                    nugetXmlDocument.Save(newNuspecFile);
                }
            }

            return context;
        }
    }
}
