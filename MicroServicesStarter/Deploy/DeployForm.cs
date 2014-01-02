namespace MicroServicesStarter.Deploy
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using Chains;
    using MicroServicesStarter.Deploy.Actions;
    using MicroServicesStarter.ServiceManagement;
    using MicroServicesStarter.ServiceManagement.Action;

    public partial class DeployForm : Form
    {
        private readonly DeploySetupContext deploySetupContext;

        private ReleaseType releaseType = ReleaseType.Debug;

        public DeployForm(AdminSetupContext adminSetupContext)
        {
            this.deploySetupContext = new DeploySetupContext(adminSetupContext);
            InitializeComponent();
        }

        private void DeployForm_Load(object sender, EventArgs e)
        {
            deploySetupContext.Parent.Do(new GatherProjectInfo());

            foreach (var project in deploySetupContext.Parent.Projects)
            {
                projectsFlowLayoutPanel.Controls.Add(new DeployItem(project));
            }

            currentVersionLabel.Text = "Current Version: " + deploySetupContext.CurrentVersion;
            updatingToLabel.Text = "To: "
                + PreparedDeploySetupContext.GetNextVersion(releaseType, deploySetupContext.CurrentVersion);
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            deploySetupContext.Parent.LogMethod = LogMethod;
            doDeployBackgroundWorker.RunWorkerAsync();
            doDeployBackgroundWorker.RunWorkerCompleted += DoDeployBackgroundWorkerOnRunWorkerCompleted;
        }

        private void DoDeployBackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            this.Close();
        }

        private void LogMethod(string text)
        {
            reportToolStripStatusLabel.Text = text;
        }

        private void debugRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            releaseType = ReleaseType.Debug;
            updatingToLabel.Text = "To: "
                + PreparedDeploySetupContext.GetNextVersion(releaseType, deploySetupContext.CurrentVersion);
        }

        private void releaseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            releaseType = ReleaseType.Release;
            updatingToLabel.Text = "To: "
                + PreparedDeploySetupContext.GetNextVersion(releaseType, deploySetupContext.CurrentVersion);
        }

        private void minorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            releaseType = ReleaseType.Minor;
            updatingToLabel.Text = "To: "
                + PreparedDeploySetupContext.GetNextVersion(releaseType, deploySetupContext.CurrentVersion);
        }

        private void majorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            releaseType = ReleaseType.Major;
            updatingToLabel.Text = "To: "
                + PreparedDeploySetupContext.GetNextVersion(releaseType, deploySetupContext.CurrentVersion);
        }

        private void doDeployBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var hasAnyNugetPackage = deploySetupContext.Parent.Projects.Any(p => p.IsNugetPackage);
            var hasAnyUpdatePackage = deploySetupContext.Parent.Projects.Any(p => p.IsUpdatePackage);

            deploySetupContext.LogToUi("Initiating nuget...").DoIf(x => hasAnyNugetPackage, new InitNuget());

            var preparedDeployContext =
                deploySetupContext.Do(new ToPreparedDeployContext(releaseType, releaseCommentsTextBox.Text));

            preparedDeployContext.LogToUi("Deploying to update...")
                                 .DoIf(x => hasAnyUpdatePackage, new PrepareUpdateFiles())
                                 .DoIfNotNull(new DeployToUpdate());

            preparedDeployContext.DoIf(x => hasAnyNugetPackage, new PrepareNugetFiles())
                                 .DoIfNotNull(new FillOutNuspecFiles())
                                 .DoIfNotNull(new DeployToNuget());

            preparedDeployContext.Do(new SaveVersion())
                                 .LogToUi(
                                     string.Format(
                                         "Version {0} has been published.", preparedDeployContext.UpdateToVersion));
        }
    }
}
