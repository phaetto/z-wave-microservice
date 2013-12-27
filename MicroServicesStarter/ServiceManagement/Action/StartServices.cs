namespace MicroServicesStarter.ServiceManagement.Action
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Chains;
    using Chains.Play;
    using Chains.Play.Web;
    using MicroServicesStarter.ServiceManagement.Debugger;
    using Services.Communication.Protocol;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Update;
    using Services.Management.Administration.Worker;

    public sealed class StartServices : IChainableAction<AdminSetupContext, AdminSetupContext>
    {
        private string jsonFileWithStartingServices;

        private readonly string adminHost;

        private readonly int adminPort;

        public StartServices(
            string jsonFileWithStartingServices = null,
            string adminHost = StartAdmin.LocalAdminHost,
            int adminPort = StartAdmin.LocalAdminPort)
        {
            this.jsonFileWithStartingServices = jsonFileWithStartingServices;
            this.adminHost = adminHost;
            this.adminPort = adminPort;
        }

        public AdminSetupContext Act(AdminSetupContext context)
        {
            jsonFileWithStartingServices = jsonFileWithStartingServices ?? string.Format("services-{0}.json", context.SetupType.ToString());

            var servicesToStart = Json<StartWorkerData[]>.Deserialize(File.ReadAllText(jsonFileWithStartingServices));

            MessageFilter.Register();
            
            using (
                var adminConnection =
                    new Client(adminHost, adminPort).Do(new WaitUntilClientConnects()))
            {
                foreach (var workerData in servicesToStart)
                {
                    try
                    {
                        context.LogToUi(
                            string.Format("Starting service {0}:{1}", workerData.ServiceName, workerData.Id));

                        if (context.SetupType == SetupType.Debug || context.SetupType == SetupType.Release)
                        {
                            // Force all applications to be in one server for testing
                            workerData.AdminHost = adminHost;
                            workerData.AdminPort = adminPort;
                        }

                        adminConnection.Do(new Send(new StartWorkerProcess(workerData)));

                        if (context.SetupType == SetupType.Debug)
                        {
                            context.LogToUi(
                                string.Format("Attaching to service {0}:{1}", workerData.ServiceName, workerData.Id));

                            var vsProcess = VisualStudioAttacher.GetVisualStudioForSolution(context.SolutionDirectory);
                            var serviceProcess = Process.GetProcessesByName("Services.Executioner").OrderByDescending(x => x.StartTime).First();
                            VisualStudioAttacher.AttachVisualStudioToProcess(vsProcess, serviceProcess);
                        }
                    }
                    catch (Exception exception)
                    {
                        context.LogToUi(string.Format("Error while starting {0}: {1}", workerData.Id, exception.Message));
                        Thread.Sleep(4000);
                    }
                }

                foreach (var workerData in servicesToStart)
                {
                    try
                    {
                        context.LogToUi(
                            string.Format("Waiting service {0}:{1} to warm up", workerData.ServiceName, workerData.Id));

                        adminConnection.Do(new WaitUntilServiceIsUp(workerData.Id));
                    }
                    catch (Exception exception)
                    {
                        context.LogToUi(string.Format("Error while starting {0}: {1}", workerData.Id, exception.Message));
                        Thread.Sleep(4000);
                    }
                }
            }

            MessageFilter.Revoke();

            return context;
        }
    }
}
