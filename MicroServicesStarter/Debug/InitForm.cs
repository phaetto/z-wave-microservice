namespace MicroServicesStarter.Debug
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;
    using Chains.Play;
    using MicroServicesStarter.Deploy;
    using MicroServicesStarter.ServiceManagement;
    using MicroServicesStarter.ServiceManagement.Action;
    using Services.Management.Administration.Worker;

    public partial class InitForm : Form
    {
        private readonly SetupType setupType;

        private AdminSetupContext adminSetupContext;

        private bool hasInitialized = false;

        public InitForm(SetupType setupType)
        {
            this.setupType = setupType;
            InitializeComponent();
        }

        private void Starter_Load(object sender, EventArgs e)
        {
            adminSetupContext = new AdminSetupContext(OnLogMethod, setupType);
        }

        private void OnLogMethod(string text)
        {
            if (reportLabel.InvokeRequired)
            {
                Invoke(new Action<string>(OnLogMethod), new object[] { text });

                return;
            }

            reportLabel.Text = text;
        }

        private void Starter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!hasInitialized)
            {
                e.Cancel = true;
                return;
            }

            switch (setupType)
            {
                case SetupType.Debug:
                case SetupType.Release:
                    new StopServicesForm(adminSetupContext).ShowDialog();
                    break;
            }
        }

        private void Starter_Shown(object sender, EventArgs e)
        {
            switch (setupType)
            {
                case SetupType.Debug:
                case SetupType.Release:
                    setupLocalEnvironmentBackgroundWorker.RunWorkerAsync();
                    break;
                case SetupType.IntegrationTest:
                    integrationTestBackgroundWorker.RunWorkerAsync();
                    break;
                case SetupType.Deploy:
                    hasInitialized = true;
                    new DeployForm(adminSetupContext).ShowDialog();
                    Close();
                    break;
            }
        }

        private void integrationTestBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var jsonFileWithAdminJson = string.Format("admin-{0}.json", setupType.ToString());
            var jsonFileWithStartingServices = string.Format("services-{0}.json", setupType.ToString());

            var adminStartData = Json<StartWorkerData>.Deserialize(File.ReadAllText(jsonFileWithAdminJson));

            adminSetupContext.LogToUi(string.Format("Installing assemblies on server tcp://{0}:{1}...", adminStartData.AdminHost, adminStartData.AdminPort))
                             .Do(new InstallProjects(adminStartData.AdminHost, adminStartData.AdminPort))
                             .LogToUi("Starting services on remote server...")
                             .Do(new StartServices(jsonFileWithStartingServices, adminStartData.AdminHost, adminStartData.AdminPort))
                             .LogToUi(string.Format("Deployed to tcp://{0}:{1}", adminStartData.AdminHost, adminStartData.AdminPort));

            hasInitialized = true;
        }

        private void setupLocalEnvironmentBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            adminSetupContext.LogToUi("Starting admin process...")
                             .Do(new StartAdmin())
                             .LogToUi("Checking projects...")
                             .Do(new CheckProjectsToPublish())
                             .LogToUi("Installing assemblies...")
                             .Do(new InstallProjects())
                             .LogToUi("Starting services...")
                             .Do(new StartServices())
                             .LogToUi("Environment is ready. Close this form to shutdown.");

            hasInitialized = true;
        }
    }
}
