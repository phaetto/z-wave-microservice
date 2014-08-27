namespace MicroServicesStarter
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using MicroServicesStarter.Debug;
    using MicroServicesStarter.ServiceManagement;
    using MicroServicesStarter.ServiceManagement.Action;
    using Services.Packages;
    using Services.Packages.Client.Actions;

    static class Program
    {
        private const string MicroServicesStarterFolder = @"..\..\";
        private const string AdminFolder = @"..\..\..\Admin\";

        [STAThread]
        static void Main(string[] args)
        {
            // This runs when the update script runs
            if (args.Length > 0 && args[0] == "--update")
            {
                UpdateApplication();

                return;
            }

#if DEBUG
            // This runs when we want to smart-debug and sniff if the debugger is attached
            if (args.Length == 0)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "MicroServicesStarter.exe",
                    Arguments = "--debug",
                    UseShellExecute = true,
                });

                return;
            }

            Thread.Sleep(100);

            // Attach on the process after debugging. This ensures that the application stays on after we stop debugging
            new AdminSetupContext().Do(new AttachDebuggerToProcess(Process.GetCurrentProcess()));
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            Application.Run(new InitForm(SetupType.Debug));
#elif DEPLOY
            Application.Run(new InitForm(SetupType.Deploy));
#elif INTEGRATIONTEST
            Application.Run(new InitForm(SetupType.IntegrationTest));
#else
            Application.Run(new InitForm(SetupType.Release));
#endif
        }

        private static void UpdateApplication()
        {
            Console.WriteLine("Updating the project...");

            UpdateOnPath(
                MicroServicesStarterFolder,
                new[]
                {
                    "Developer.MicroServicesStarter"
                });

            Console.WriteLine("Updating the admin...");

            UpdateOnPath(
                AdminFolder,
                new[]
                {
                    "Services.Executioner"
                });

            Console.WriteLine("Done!");
        }

        private static void UpdateOnPath(string relativePath, string[] packages)
        {
            var fullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var applicationRepository = new Repository(fullPath);

            foreach (var package in packages)
            {
                applicationRepository.RegisterPackage(package);
            }

            applicationRepository.Do(new UpdateClientApplication("update.msd.am", 12345, fullPath));
        }
    }
}
