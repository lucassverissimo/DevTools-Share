using DTSWindowsForm.UI.Dashboard;
using DTSWindowsForm.UI.FattureWeb;

namespace DTSWindowsForm
{
    public partial class FrmDTS : Form
    {
        public FrmDTS()
        {
            InitializeComponent();
        }
        private void OpenForm<T>() where T : Form, new()
        {
            var form = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (form != null)
            {
                form.Activate();
                return;
            }
            form = new T();
            form.Show();
        }
        public static void MethodNotImplemented()
        {
            MessageBox.Show("👷🚧 Em construção!!!", "Método não implementado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void btnFattureWeb_Click(object sender, EventArgs e)
        {
            OpenForm<FattureWebForm>();
        }

        private void btnBancoLocal_Click(object sender, EventArgs e)
        {
            MethodNotImplemented();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            OpenForm<DashboardForm>();
        }
    }
}
