using MIGAZ;
using System;
using System.Windows.Forms;

namespace MIGAZ.Forms
{
    public partial class formOptions : Form
    {
        public formOptions()
        {
            InitializeComponent();
        }

        private void chkAllowTelemetry_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowTelemetry.Checked == true)
            {
                string message = "" + "\n";
                message = "\n" + "Tool telemetry data is important for us to keep improving it. Data collected is for tool development usage only and will not be shared, by any reason, out of the tool development team or scope.";
                message += "\n";
                message += "\n" + "Tool telemetry DOES send:";
                message += "\n" + ". TenantId";
                message += "\n" + ". SubscriptionId";
                message += "\n" + ". Processed resources type";
                message += "\n" + ". Processed resources location";
                message += "\n" + ". Execution date";
                message += "\n";
                message += "\n" + "Tool telemetry DOES NOT send:";
                message += "\n" + ". Resources names";
                message += "\n" + ". Any resources configuration or caracteristics";
                message += "\n" + ". Any local computer information";
                message += "\n" + ". Any other information not stated on the \"Tool telemetry DOES send\" section";
                message += "\n";
                message += "\n" + "Do you authorize the tool to send telemetry data?";
                DialogResult dialogresult = MessageBox.Show(message, "Authorization Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogresult == DialogResult.No)
                {
                    chkAllowTelemetry.Checked = false;
                }
            }
        }

        private void txtSuffix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            app.Default.UniquenessSuffix = txtSuffix.Text;
            app.Default.BuildEmpty = chkBuildEmpty.Checked;
            app.Default.AutoSelectDependencies = chkAutoSelectDependencies.Checked;
            app.Default.SaveSelection = chkSaveSelection.Checked;
            app.Default.AllowTelemetry = chkAllowTelemetry.Checked;
            app.Default.AzureEnvironment = cboAzureEnvironment.Text;
            app.Default.Save();
        }

        private void formOptions_Load(object sender, EventArgs e)
        {
            txtSuffix.Text = app.Default.UniquenessSuffix;
            chkBuildEmpty.Checked = app.Default.BuildEmpty;
            chkAutoSelectDependencies.Checked = app.Default.AutoSelectDependencies;
            chkSaveSelection.Checked = app.Default.SaveSelection;
            chkAllowTelemetry.Checked = app.Default.AllowTelemetry;
            cboAzureEnvironment.Text = app.Default.AzureEnvironment;
        }
    }
}
