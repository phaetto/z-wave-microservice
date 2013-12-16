namespace MicroServicesStarter
{
    using System;
    using System.Windows.Forms;
    using MicroServicesStarter.Debug;

    static class Program
    {
        [STAThread]
        static void Main()
        {
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
