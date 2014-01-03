namespace MicroServicesStarter.ServiceManagement.Action
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Chains;
    using Chains.Play;
    using Chains.Play.Web;
    using Ionic.Zip;
    using Services.Communication.Protocol;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Update;
    using Services.Management.Administration.Worker;

    public sealed class InstallAndStartServicesForIntegrationTest : IChainableAction<AdminSetupContext, AdminSetupContext>
    {
        private string jsonFileWithStartingServices;

        public InstallAndStartServicesForIntegrationTest(string jsonFileWithStartingServices = null)
        {
            this.jsonFileWithStartingServices = jsonFileWithStartingServices;
        }

        public AdminSetupContext Act(AdminSetupContext context)
        {
            jsonFileWithStartingServices = jsonFileWithStartingServices ?? string.Format("services-{0}.json", context.SetupType.ToString());

            var servicesToStart = Json<StartWorkerData[]>.Deserialize(File.ReadAllText(jsonFileWithStartingServices));

            foreach (var workerData in servicesToStart)
            {
                using (var adminConnection = new Client(workerData.AdminHost, workerData.AdminPort).Do(new OpenConnection()))
                {
                    var reports = adminConnection.Do(new Send<GetReportedDataReturnData>(new GetReportedData())).Reports;
                    var repository =
                        adminConnection.Do(new Send<GetAllRepoServicesReturnData>(new GetAllRepoServices())).RepoServices;

                    var project = context.Projects.First(p => p.Name == workerData.ServiceName);

                    if (repository.ContainsKey(project.Name))
                    {
                        context.LogToUi(string.Format("Housekeeping on service {0}...", project.Name));

                        if (
                            reports.Any(
                                x =>
                                    x.Value.StartData.ServiceName == project.Name
                                        && x.Value.WorkerState == WorkUnitState.Running))
                        {
                            // Stop the services
                            context.LogToUi(string.Format("Stopping instances of service {0}", project.Name));

                            reports.Where(
                                x =>
                                    x.Value.StartData.ServiceName == project.Name
                                        && x.Value.WorkerState == WorkUnitState.Running)
                                   .Select(
                                       x =>
                                           adminConnection.Do(new Send(new StopWorkerProcess(x.Key)))
                                                          .Do(new WaitUntilServiceIsDown(x.Key))
                                                          .Do(new Send(new DeleteWorkerProcessEntry(x.Key))))
                                   .ToList();
                        }

                        context.LogToUi(string.Format("Uninstalling service {0}", project.Name));

                        adminConnection.Do(new Send(new UninstallService(project.Name)))
                                       .Do(new WaitUntilServiceIsUninstalled(project.Name));
                    }

                    context.LogToUi(string.Format("Zipping file for {0}...", project.Name));

                    // Install the service
                    var zipName = string.Format("{0}.archive.zip", project.Name);

                    using (var zip = new ZipFile())
                    {
                        zip.AddDirectory(project.BinDirectory);
                        zip.Save(zipName);
                    }

                    var fileUploadToAdminData = new FileUploadToAdminData
                    {
                        FileData = File.ReadAllBytes(zipName),
                        ServiceName = project.Name
                    };

                    File.Delete(zipName);

                    context.LogToUi(string.Format("Uploading and installing service {0}...", project.Name));

                    adminConnection.Do(new Send(new UploadZipAndApplyServiceVersionUpdateFromIt(fileUploadToAdminData)));

                    try
                    {
                        context.LogToUi(
                            string.Format("Starting service {0}:{1}", workerData.ServiceName, workerData.Id));

                        adminConnection.Do(new Send(new StartWorkerProcess(workerData)));
                    }
                    catch (Exception exception)
                    {
                        context.LogToUi(
                            string.Format("Error while starting {0}: {1}", workerData.Id, exception.Message));
                        Thread.Sleep(4000);
                    }
                }
            }

            return context;
        }
    }
}
