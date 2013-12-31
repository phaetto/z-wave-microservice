namespace MicroServicesStarter
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using MicroServicesStarter.Debug;
    using Services.Packages;
    using Services.Packages.Client.Actions;

    static class Program
    {
        private const string MicroServicesStarterFolder = @"..\..\";
        private const string AdminFolder = @"..\..\..\Admin\";

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--update")
            {
                UpdateApplication();

                return;
            }

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
