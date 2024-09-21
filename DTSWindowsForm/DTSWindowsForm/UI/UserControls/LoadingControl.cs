namespace DTSWindowsForm.UI.UserControls
{
    public partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill; // Para preencher o formulário
        }

        public void ShowLoading(Control parent, string message)
        {
            parent.Controls.Add(this);
            //parent.Enabled = false; // Desabilitar o formulário
            this.Visible = true;

            // Definir a mensagem
            labelMessage.Text = message; // Supondo que você tenha um Label chamado labelMessage
            this.Refresh();
            this.BringToFront();
        }

        public void HideLoading(Control parent)
        {
            parent.Enabled = true; // Habilitar o formulário
            parent.Controls.Remove(this);
            this.Visible = false;
        }
    }
}
