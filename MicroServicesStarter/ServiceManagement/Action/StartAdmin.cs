namespace MicroServicesStarter.ServiceManagement.Action
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Sockets;
    using Chains;
    using Chains.Play;
    using Chains.Play.Web;
    using Services.Communication.Protocol;
    using Services.Management.Administration.Worker;

    public sealed class StartAdmin : IChainableAction<AdminSetupContext, AdminSetupContext>
    {
        public const string LocalAdminHost = "localhost";

        public const int LocalAdminPort = 12099;

        public AdminSetupContext Act(AdminSetupContext context)
        {
            try
            {
                using (new Client(LocalAdminHost, LocalAdminPort).Do(new OpenConnection()))
                {
                    // Admin is open
                }
            }
            catch (SocketException)
            {
                var jsonFileWithAdminJson = string.Format("admin-{0}.json", context.SetupType.ToString());

                var adminStartData = Json<StartWorkerData>.Deserialize(File.ReadAllText(jsonFileWithAdminJson));
                adminStartData.AdminHost = "0.0.0.0";
                adminStartData.AdminPort = LocalAdminPort;

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = context.SolutionDirectory + @"\Admin\Services.Executioner.exe",
                    Arguments = "--admin " + adminStartData.SerializeToJsonForCommandPrompt(),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = context.SolutionDirectory + @"\Admin\",
                };

                context.AdminProcess = Process.Start(processStartInfo);
            }

            return context;
        }
    }
}
