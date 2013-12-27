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

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--update")
            {
                var fullPath =
                    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MicroServicesStarterFolder));

                var applicationRepository = new Repository(fullPath);

                applicationRepository.Do(
                    new UpdateClientApplication("update.msd.am", 12345, fullPath));

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
    }
}
