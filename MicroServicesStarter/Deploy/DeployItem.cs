namespace MicroServicesStarter.Deploy
{
    using System.Windows.Forms;

    public partial class DeployItem : UserControl
    {
        public DeployItem(ProjectRegistration projectRegistration)
        {
            InitializeComponent();

            nameLabel.Text = projectRegistration.Name;

            if (projectRegistration.IsNugetPackage)
            {
                typeLabel.Text = "Nuget";
            }
            else if (projectRegistration.IsUpdatePackage)
            {
                typeLabel.Text = "Update";
            }
            else
            {
                typeLabel.Text = "None";
            }
        }
    }
}
