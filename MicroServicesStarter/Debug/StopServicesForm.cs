namespace MicroServicesStarter.Debug
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using MicroServicesStarter.ServiceManagement;
    using MicroServicesStarter.ServiceManagement.Action;

    public partial class StopServicesForm : Form
    {
        private readonly AdminSetupContext adminSetupContext;

        private bool hasFinished = false;

        public StopServicesForm(AdminSetupContext adminSetupContext)
        {
            this.adminSetupContext = adminSetupContext;
            this.adminSetupContext.LogMethod = OnLogMethod;
            InitializeComponent();
        }

        private void stopServicesBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            adminSetupContext.LogToUi("Closing stuff...")
                             .Do(new CloseServices())
                             .LogToUi("Closing application and admin");

            if (adminSetupContext.AdminProcess != null)
            {
                adminSetupContext.AdminProcess.Kill();
            }

            hasFinished = true;

            CloseDialog();
        }

        private void StopServices_Load(object sender, EventArgs e)
        {
            stopServicesBackgroundWorker.RunWorkerAsync();
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

        private void CloseDialog()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(CloseDialog));

                return;
            }

            Close();
        }

        private void StopServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !hasFinished;
        }
    }
}
