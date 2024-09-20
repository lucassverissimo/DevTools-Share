using System.Data;
using System.Data.SqlClient;

namespace DTSWindowsForm.UI.Dashboard
{
    public partial class DashboardForm : Form
    {
        private string query;
        public DashboardForm()
        {
            InitializeComponent();
            this.KeyPreview = true;

            // Associa o evento KeyDown ao método Form_KeyDown
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica se a tecla pressionada foi F5
            if (e.KeyCode == Keys.F5)
            {
                // Chama o método de clique do botão "Executar"
                btnExecutar.PerformClick();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (string.IsNullOrEmpty(rtxConsulta.Text))
            {
                return;
            }

            string connStr = Program.AppSettings.Connection;

            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            ConsultaGrid.DataSource = null;
                            ConsultaGrid.DataSource = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TimerReload.Stop();
                    MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxConsulta.Text))
            {
                MessageBox.Show($"Não há o que executar!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtmReload.Text))
            {
                TimerReload.Stop();
            }
            else
            {
                TimerReload.Interval = int.Parse(txtmReload.Text) * 1000;
                TimerReload.Start();
            }
            query = rtxConsulta.Text;
            LoadData();
        }
    }
}

