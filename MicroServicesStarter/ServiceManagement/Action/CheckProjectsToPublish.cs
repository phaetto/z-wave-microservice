﻿namespace MicroServicesStarter.ServiceManagement.Action
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Chains;

    public sealed class CheckProjectsToPublish : IChainableAction<AdminSetupContext, AdminSetupContext>
    {
        public AdminSetupContext Act(AdminSetupContext context)
        {
            var projectDirectories = Directory.GetDirectories(context.SolutionDirectory);

            var binDirectory = context.SetupType.ToString();
            context.Projects.Clear();

            foreach (var projectDirectory in projectDirectories)
            {
                var projectBinDirectory = string.Format(@"{0}\bin\{1}\", projectDirectory, binDirectory);
                if (!Directory.Exists(projectBinDirectory))
                {
                    continue;
                }

                var folderName = projectDirectory.Substring(context.SolutionDirectory.Length);

                var nuspecFile = string.Format(@"{0}\{1}.nuspec", projectDirectory, folderName);
                var updateFile = string.Format(@"{0}\update.json", projectDirectory);

                context.Projects.Add(new ProjectRegistration
                {
                    Directory = projectDirectory + @"\",
                    BinDirectory = projectBinDirectory,
                    Name = folderName,
                    IsNugetPackage = File.Exists(nuspecFile),
                    IsUpdatePackage = File.Exists(updateFile),
                });
            }

            return context;
        }
    }
}
